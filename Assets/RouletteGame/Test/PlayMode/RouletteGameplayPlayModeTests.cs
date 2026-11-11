#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using RouletteGame.Core;
using Object = UnityEngine.Object;

namespace RouletteGame.Tests.PlayMode
{
    /// <summary>
    /// Plays the REAL Roulette scene: it clicks the real buttons and checks the real wallet, history, result card and the
    /// real position of the ball on the wheel.
    ///
    /// The game scripts live in Assembly-CSharp, which a test assembly cannot reference, so the tests reach them by
    /// reflection. They rely on the default settings asset (first chip = 10, starting balance = 1000).
    ///
    /// SAFETY: the scene's SaveManager is pointed at a separate test file BEFORE it starts, so your real save file
    /// (balance, histories) is never touched.
    /// </summary>
    public class RouletteGameplayPlayModeTests
    {
        private const string TestSaveName = "roulette_playmode_test.json";
        private const float WaitSeconds = 45f;

        private MonoBehaviour controller, wallet, history, resultPanel, wheelView;

        // ---------- Setup ----------

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            DeleteTestFiles();

            SceneManager.sceneLoaded += RedirectSave;
            AsyncOperation load = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(
    FindScenePath(), new LoadSceneParameters(LoadSceneMode.Single));
yield return load;
            SceneManager.sceneLoaded -= RedirectSave;

            yield return null;
            yield return null;
            yield return null;

            controller = FindBehaviour("RouletteGameController");
            wallet = FindBehaviour("VirtualWalletManager");
            history = FindBehaviour("RouletteHistoryManager");
            resultPanel = FindBehaviour("RouletteResultPanel");
            wheelView = FindBehaviour("WheelView");

            float until = Time.realtimeSinceStartup + 5f;
            while (Balance() != 1000 && Time.realtimeSinceStartup < until) yield return null;
            Assert.AreEqual(1000, Balance(), "A fresh test game should start with 1000 credits.");
            Assert.AreEqual("Betting", StateName());
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return null;
            DeleteTestFiles();
        }

        // Runs after the scene's Awake and before its first Start: the SaveManager reads this name in Start.
        private static void RedirectSave(Scene scene, LoadSceneMode mode)
{
    foreach (MonoBehaviour behaviour in Object.FindObjectsByType<MonoBehaviour>(
        FindObjectsInactive.Include,
        FindObjectsSortMode.None))
    {
        if (behaviour == null || behaviour.GetType().Name != "SaveManager")
            continue;

        FieldInfo field = behaviour.GetType().GetField(
            "saveFileName",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        Assert.IsNotNull(field, "SaveManager has no 'saveFileName' field.");

        field.SetValue(behaviour, TestSaveName);
    }
}

        private static void DeleteTestFiles()
        {
            string basePath = Path.Combine(Application.persistentDataPath, TestSaveName);
            foreach (string path in new[] { basePath, basePath + ".bak", basePath + ".tmp", basePath + ".corrupt" })
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static string FindScenePath()
        {
            foreach (string guid in UnityEditor.AssetDatabase.FindAssets("RouletteGame t:Scene"))
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileName(path) == "RouletteGame.unity") return path;
            }
            Assert.Fail("RouletteGame.unity was not found. Build it with Tools > Roulette Game > Build Roulette Scene.");
            return null;
        }

        // ---------- Tests ----------

        [UnityTest, Timeout(120000)]
        public IEnumerator WinningRound_IsSettledOnce_BeforeTheAnimation_AndShowsTheResult()
        {
            Controller("SetOutcomeSource", new ScriptedOutcomeSource(17));
            Click("Number_17");
            Click("ConfirmButton");
            Assert.AreEqual("Confirmed", StateName());
            Assert.AreEqual(1000, Balance(), "CONFIRM must not charge anything.");

            Click("SpinButton");

            // The wallet is already settled while the wheel is still turning: stake 10 taken, 360 returned (350 + stake).
            Assert.AreEqual("Spinning", StateName());
            Assert.AreEqual(1000 - 10 + 360, Balance());

            yield return FastForwardToResult();

            Assert.AreEqual(17, (int)Property(wheelView, "LastNumber"));
            Assert.IsTrue((bool)Property(resultPanel, "IsOpen"), "The result card should be open.");
            Assert.AreEqual("17", TextOf(resultPanel, "numberText"));
            Assert.AreEqual("BLACK", TextOf(resultPanel, "colorText"));
            Assert.AreEqual(1350, Balance());

            // One BET and one WIN for this round, sharing one round id. Nothing duplicated.
            List<(string type, string round, long amount)> ledger = Ledger();
            Assert.AreEqual(3, ledger.Count, "starting balance + bet + win");
            Assert.AreEqual("Bet", ledger[1].type);
            Assert.AreEqual(-10, ledger[1].amount);
            Assert.AreEqual("Win", ledger[2].type);
            Assert.AreEqual(360, ledger[2].amount);
            Assert.AreEqual(ledger[1].round, ledger[2].round);
            Assert.IsNotEmpty(ledger[1].round);

            IReadOnlyList<RouletteRoundRecord> rounds = Rounds();
            Assert.AreEqual(1, rounds.Count);
            Assert.AreEqual(17, rounds[0].winningNumber);
            Assert.AreEqual(10, rounds[0].totalBet);
            Assert.AreEqual(360, rounds[0].totalReturned);
            Assert.AreEqual(ledger[1].round, rounds[0].roundId);

            Click("ContinueButton");
            Assert.AreEqual("Betting", StateName());
            Assert.AreEqual(1350, Balance());
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator LosingRound_ChargesTheStakeAndPaysNothing()
        {
            yield return PlayRound(18, "Number_17");

            Assert.AreEqual(990, Balance());
            List<(string type, string round, long amount)> ledger = Ledger();
            Assert.AreEqual(2, ledger.Count, "starting balance + bet only");
            Assert.AreEqual("Bet", ledger[1].type);
            Assert.AreEqual(1, Rounds().Count);
            Assert.AreEqual(-10, Rounds()[0].net);
            Assert.AreEqual("18", TextOf(resultPanel, "numberText"));
            yield return null;
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Betting_ClosesWhenTheSpinStarts_AndBetsCannotChangeDuringIt()
        {
            Controller("SetOutcomeSource", new ScriptedOutcomeSource(5));
            Click("Number_17");
            Click("ConfirmButton");
            Click("SpinButton");
            Assert.AreEqual("Spinning", StateName());

            // Every way of changing the bets is refused while the wheel is turning.
            Assert.IsFalse(Button("ConfirmButton").interactable);
            Assert.IsFalse(Button("UndoButton").interactable);
            Assert.IsFalse(Button("ClearButton").interactable);
            Assert.IsFalse(Button("RepeatButton").interactable);
            Assert.IsFalse(Button("RemoveButton").interactable);
            Click("Number_5");          // a tap on the table
            Click("Number_36");
            Assert.AreEqual(1000 - 10, Balance(), "Nothing more may be charged.");

            yield return FastForwardToResult();

            RouletteRoundRecord record = Rounds()[0];
            Assert.AreEqual(1, record.bets.Count, "Taps during the spin must not become bets.");
            Assert.AreEqual(10, record.totalBet);
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator ConfirmedTable_IsLocked_UntilEditBetsReopensIt()
        {
            Controller("SetOutcomeSource", new ScriptedOutcomeSource(1));
            Click("Number_17");
            Click("ConfirmButton");
            Assert.AreEqual("Confirmed", StateName());

            Click("Number_5");          // refused: the table is locked
            Assert.IsFalse(Button("UndoButton").interactable);

            Click("ConfirmButton");     // now reads EDIT BETS
            Assert.AreEqual("Betting", StateName());
            Assert.IsFalse(Button("SpinButton").interactable, "SPIN needs a confirmed table.");

            Click("Number_5");          // accepted
            Click("ConfirmButton");
            Click("SpinButton");
            yield return FastForwardToResult();

            Assert.AreEqual(20, Rounds()[0].totalBet, "Only the two accepted chips count (17 and 5), not the refused tap.");
            Assert.AreEqual(2, Rounds()[0].bets.Count);
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator NotEnoughCredits_BlocksTheBet()
        {
            Assert.IsTrue((bool)wallet.GetType().GetMethod("SpendCredits").Invoke(wallet, new object[] { 995L, "TEST-SETUP" }));
            yield return null;
            Assert.AreEqual(5, Balance());

            Click("Number_17");         // a chip of 10 costs more than the 5 credits left
            yield return null;

            Assert.IsFalse(Button("ConfirmButton").interactable, "There is no bet to confirm.");
            Assert.IsFalse(Button("SpinButton").interactable);
            Assert.AreEqual(5, Balance());
            Assert.AreEqual(0, Rounds().Count);
        }

        [UnityTest, Timeout(180000)]
        public IEnumerator NewRoundsCanFollow_EachWithItsOwnId_AndRepeatReplaysTheLastBets()
        {
            yield return PlayRound(17, "Number_17");        // win: 1000 - 10 + 360
            Assert.AreEqual(1350, Balance());

            Assert.IsTrue(Button("RepeatButton").interactable, "REPEAT should be ready after a round.");
            Click("RepeatButton");
            Controller("SetOutcomeSource", new ScriptedOutcomeSource(2));
            Click("ConfirmButton");
            Click("SpinButton");
            yield return FastForwardToResult();             // lose: -10
            Click("ContinueButton");

            Assert.AreEqual(1340, Balance());
            IReadOnlyList<RouletteRoundRecord> rounds = Rounds();
            Assert.AreEqual(2, rounds.Count);
            Assert.AreNotEqual(rounds[0].roundId, rounds[1].roundId);
            Assert.AreEqual(10, rounds[1].totalBet);

            int bets = 0, wins = 0;
            foreach ((string type, string round, long amount) entry in Ledger())
            {
                if (entry.type == "Bet") bets++;
                if (entry.type == "Win") wins++;
            }
            Assert.AreEqual(2, bets);
            Assert.AreEqual(1, wins);
        }

        [UnityTest, Timeout(240000)]
        public IEnumerator TheBall_RestsInThePocketOfTheSettledNumber_OnScreen()
        {
            foreach (int number in new[] { 0, 36, 26, 13 })
            {
                yield return PlayRound(number, "Red");

                Assert.AreEqual(number, (int)Property(wheelView, "LastNumber"));

                // Read where the ball REALLY is on screen, and which pocket of the turning wheel is under it.
                RectTransform wheel = (RectTransform)Field(wheelView, "wheel");
                RectTransform ball = (RectTransform)Field(wheelView, "ball");
                double wheelAngle = -wheel.localEulerAngles.z;                                   // clockwise from the top
                double ballAngle = Math.Atan2(ball.anchoredPosition.x - wheel.anchoredPosition.x,
                                              ball.anchoredPosition.y - wheel.anchoredPosition.y) * 180.0 / Math.PI;

                Assert.AreEqual(number, WheelTrajectory.PocketUnderBall(wheelAngle, ballAngle), "number " + number);
                Assert.AreEqual(number.ToString(), TextOf(resultPanel, "numberText"));
            }
        }

        // ---------- Round helpers ----------

        // Places one chip on each area, confirms, spins, fast-forwards, and presses CONTINUE.
        private IEnumerator PlayRound(int winningNumber, params string[] areas)
        {
            Controller("SetOutcomeSource", new ScriptedOutcomeSource(winningNumber));
            foreach (string area in areas) Click(area);
            Click("ConfirmButton");
            Click("SpinButton");
            yield return FastForwardToResult();
            if (Button("ContinueButton") != null && (bool)Property(resultPanel, "IsOpen")) Click("ContinueButton");
            Assert.AreEqual("Betting", StateName());
        }

        // Pressing SPIN again during the spin fast-forwards the animation (the round is already settled).
        private IEnumerator FastForwardToResult()
        {
            if (StateName() == "Spinning") Click("SpinButton");

            float until = Time.realtimeSinceStartup + WaitSeconds;
            while (StateName() != "ShowingResult")
            {
                if (Time.realtimeSinceStartup > until) Assert.Fail("The round did not reach the result in " + WaitSeconds + " seconds. State: " + StateName());
                yield return null;
            }
        }

        // ---------- Reflection helpers ----------

        private static MonoBehaviour FindBehaviour(string typeName)
{
    foreach (MonoBehaviour behaviour in Object.FindObjectsByType<MonoBehaviour>(
        FindObjectsInactive.Include,
        FindObjectsSortMode.None))
    {
        if (behaviour != null && behaviour.GetType().Name == typeName)
            return behaviour;
    }

    Assert.Fail("No " + typeName + " in the scene.");
    return null;
}

        private static Button Button(string objectName)
        {
            foreach (Button button in Resources.FindObjectsOfTypeAll<Button>())
            {
                if (button.name == objectName && button.gameObject.scene.IsValid() && button.gameObject.scene.isLoaded) return button;
            }
            return null;
        }

        private static void Click(string objectName)
        {
            Button button = Button(objectName);
            Assert.IsNotNull(button, "Button '" + objectName + "' was not found.");
            button.onClick.Invoke();
        }

        private void Controller(string method, object argument)
        {
            MethodInfo info = controller.GetType().GetMethod(method);
            Assert.IsNotNull(info, "Controller has no method " + method);
            info.Invoke(controller, new[] { argument });
        }

        private string StateName()
        {
            return Property(controller, "State").ToString();
        }

        private long Balance()
        {
            return (long)wallet.GetType().GetMethod("GetBalance").Invoke(wallet, null);
        }

        private IReadOnlyList<RouletteRoundRecord> Rounds()
        {
            return (IReadOnlyList<RouletteRoundRecord>)Property(history, "Rounds");
        }

        private List<(string type, string round, long amount)> Ledger()
        {
            List<(string, string, long)> list = new List<(string, string, long)>();
            foreach (object tx in (IEnumerable)Property(wallet, "Transactions"))
            {
                Type t = tx.GetType();
                list.Add((t.GetField("type").GetValue(tx).ToString(), (string)t.GetField("gameRoundId").GetValue(tx), (long)t.GetField("amount").GetValue(tx)));
            }
            return list;
        }

        private static object Property(object target, string name)
        {
            PropertyInfo info = target.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.IsNotNull(info, target.GetType().Name + " has no property " + name);
            return info.GetValue(target);
        }

        private static object Field(object target, string name)
        {
            FieldInfo info = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.IsNotNull(info, target.GetType().Name + " has no field " + name);
            return info.GetValue(target);
        }

        private static string TextOf(object owner, string fieldName)
        {
            object text = Field(owner, fieldName);
            return (string)text.GetType().GetProperty("text").GetValue(text);
        }
    }
}
#endif
