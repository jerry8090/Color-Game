#include "pch-cpp.hpp"





template <typename T1>
struct VirtualActionInvoker1
{
	typedef void (*Action)(void*,T1,const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj,p1,invokeData.method);
	}
};
template <typename T1>
struct VirtualActionInvoker1Invoker;
template <typename T1>
struct VirtualActionInvoker1Invoker<T1*>
{
	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1* p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		void* params[1] = { p1 };
		invokeData.method->invoker_method(il2cpp_codegen_get_method_pointer(invokeData.method), invokeData.method, obj, params, params[0]);
	}
};
template <typename T1, typename T2, typename T3, typename T4>
struct VirtualActionInvoker4Invoker;
template <typename T1, typename T2, typename T3, typename T4>
struct VirtualActionInvoker4Invoker<T1*, T2, T3, T4*>
{
	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1* p1, T2 p2, T3 p3, T4* p4)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		void* params[4] = { p1, &p2, &p3, p4 };
		invokeData.method->invoker_method(il2cpp_codegen_get_method_pointer(invokeData.method), invokeData.method, obj, params, params[3]);
	}
};
template <typename R>
struct VirtualFuncInvoker0
{
	typedef R (*Func)(void*,const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj,invokeData.method);
	}
};
template <typename R, typename T1>
struct VirtualFuncInvoker1
{
	typedef R (*Func)(void*,T1,const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj,p1,invokeData.method);
	}
};
template <typename R, typename T1, typename T2, typename T3>
struct VirtualFuncInvoker3
{
	typedef R (*Func)(void*,T1,T2,T3,const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2, T3 p3)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj,p1,p2,p3,invokeData.method);
	}
};
template <typename T1>
struct InterfaceActionInvoker1
{
	typedef void (*Action)(void*,T1,const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj,p1,invokeData.method);
	}
};
template <typename T1>
struct InvokerActionInvoker1;
template <typename T1>
struct InvokerActionInvoker1<T1*>
{
	static inline void Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1)
	{
		void* params[1] = { p1 };
		method->invoker_method(methodPtr, method, obj, params, params[0]);
	}
};

struct U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402;
struct U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45;
struct U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807;
struct U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553;
struct U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE;
struct U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00;
struct U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5;
struct U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6;
struct Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C;
struct Action_1_t3DC3411926243F1DB9C330F8E105B904E38C1A0B;
struct Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A;
struct Action_1_t17E52B12DC24FA6C9DD52F87043C85BEA889BB81;
struct Action_1_t2EDB30EAB747FDF563DD6410FC76AF861A09A0C2;
struct Action_1_t84D0CA347FC997E1202ECA3ED828B057841444EF;
struct Action_1_tC8822DDEF41267DA3844DAD787ACE63C0C385E89;
struct Action_1_tAE453A6458E1DAD68CAF44DC8FF62EC18FEAA930;
struct Action_2_tF46B14C98A24F40F2279A1D4296BB9078938C034;
struct Action_2_t115BA48255E00E3E7D79535060D729C4822CAFF3;
struct Action_2_t302322518DED0A32BC10F069AAEE117BC9C20917;
struct Action_2_t09DA61027B1820298B3AEBAB627FBF4C0CCC66B4;
struct Action_3_tCDDEBF125C30A90B3A5061DE417B889F78E7DB83;
struct Action_3_tAD728960C80D3C14B956508C335D759770FE2F6E;
struct Action_3_t57DE42DCD9F152289CA8303B0B1AB7246E7FE864;
struct Action_3_tE77469DC1E6595CCDCD9A1404CBA045A1C0AA560;
struct BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7;
struct BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507;
struct BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B;
struct BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F;
struct BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E;
struct Dictionary_2_t9FA6D82CAFC18769F7515BB51D1C56DAE09381C3;
struct Dictionary_2_tBCCCFBCAC02A3C03E3C84D75696D4860D7444A35;
struct Dictionary_2_tE1603CE612C16451D1E56FF4D4859D4FE4087C28;
struct Dictionary_2_t4055F6540F36F21F9FEDAFB92D8E0089B38EBBC8;
struct EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07;
struct EventCallback_1_t5B1D52718EA71C8F33D11D62351463B137A59482;
struct Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED;
struct Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18;
struct Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD;
struct Func_1_t2BE7F58348C9CC544A8973B3A9E55541DE43C457;
struct Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780;
struct Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7;
struct Func_1_t9EB8CE9DFD9B703BC79F2087B16EA394B7A9F9A1;
struct Func_1_t58C51DB29153B53A9136AE397958F3FCC1F596EC;
struct Func_1_t704C051013549CDD77A31AEC405EA270221633B3;
struct Func_1_t566D4A654882E282E54CDC3A001E05486F913903;
struct Func_1_t87EB6A475C10479F9DA4442B05AC1022C1B7419C;
struct Func_2_t2A7432CC4F64D0DF6D8629208B154CF139B39AF2;
struct Func_2_t33ED521BE3A7E943FA8D764514952EDF1AF1C0FA;
struct Func_2_t5FAD225BE5BF75673982C9FE55604AC36DBC1DB4;
struct Func_2_t45386234ABB726FFBC7310E0192AE01A58728C52;
struct Func_2_tDDC415B0A5306A4473D2ADD7C4E25824337698DB;
struct List_1_t592DF2E59D6E244DA0041698383484A07C7E0480;
struct List_1_t96E9133B70FB6765E6B138E810D33E18901715DA;
struct List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD;
struct List_1_tEA16F82F7871418E28EB6F551D77A8AD9F2E337F;
struct List_1_t6115BBE78FE9310B180A2027321DF46F2A06AC95;
struct ObjectPool_1_t6EA833FFCBB94CBB4B4ECF190C617D0010F43887;
struct RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014;
struct RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7;
struct RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7;
struct TextInputBase_t1C4ADC0DE8BF430AA17D822D068E2204405C139A;
struct TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2;
struct TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B;
struct VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87;
struct VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D;
struct VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3;
struct VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F;
struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031;
struct CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB;
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771;
struct FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F;
struct Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C;
struct IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832;
struct PropertyPathPartU5BU5D_t7994D542F14DDDDEABB1792C335C20149399AEBB;
struct SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C;
struct StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF;
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;
struct TypeU5BU5D_t97234E1129B564EB38B8D85CAC2AD8B5B9522FFB;
struct Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07;
struct BaseFieldMouseDragger_t6BB06D66A91C4C7DF3B50011F54727FB8CE92ED7;
struct BaseVisualElementPanel_tE3811F3D1474B72CB6CD5BCEECFF5B5CBEC1E303;
struct Binder_t91BFCE95A7057FADF4D8A1A342AFE52872246235;
struct Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B;
struct CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4;
struct CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57;
struct CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D;
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0;
struct DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A;
struct DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E;
struct Event_tEBC6F24B56CE22B9C9AD1AC6C24A6B83BC3860CB;
struct EventCallbackRegistry_tE18297C3F7E535BD82EDA83EC6D6DAA386226B85;
struct FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0;
struct FocusChangeDirection_t95F4784CF68331792EB9077AEAB01E17822991EF;
struct FocusController_t5D2E45F2CCBE3B7082DE4088EE03C2E8F736011A;
struct FocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE;
struct Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0;
struct IBinding_t02FD99E9C9F2072B28E110F16C157666F5B2DBC7;
struct IDictionary_t6D03155AF1FA9083817AA5B6AD7DEEACC26AB220;
struct IEventHandler_tB1627CA1B7729F3E714572E69A79C91A1578C9A3;
struct IFormatProvider_tC202922D43BFF3525109ABF3FB79625F5646AB52;
struct IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7;
struct IVisualElementScheduledItem_t309F1A5445514122A9E3F64182D0D8A4DE34C48F;
struct IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82;
struct InlineStyleAccess_t5CA7877999C9442491A220AE50D605C84D09A165;
struct Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70;
struct MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553;
struct MethodInfo_t;
struct NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472;
struct PathRef_t76F7677792A14AF9D6A6EAB7F08C1A3DC2B27A55;
struct Regex_tE773142C2BE45C5D362B0F815AFF831707A51772;
struct RenderData_t1ABE116B2B5E0409AC699E195922516606531DC2;
struct ResolvedStyleAccess_t226CC840EBACEE31CE1139ED5F717532AFFAEB45;
struct SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6;
struct String_t;
struct StringBuilder_t;
struct StyleVariableContext_tF74F2787CE1F6BEBBFBFF0771CF493AC9E403527;
struct TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4;
struct Type_t;
struct VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115;
struct VisualElementTransformData_t3DD575B5990B68FF956673EFF036171C86A38DF3;
struct VisualTreeAsset_tFB5BF81F0780A412AE5A7C2C552B3EEA64EA2EEB;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;
struct IContainer_tBD9F21C42D4253E306C4EF7CFC72480E0C7C89B5;
struct Panel_t3A0D2006E8AEA607A6DF5188138E463A26085295;
struct Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF;
struct EqualityComparer_tF8FCE99C5DBB2F35D74728CB65346435381CFD5B;
struct Data_t6BD087CC0FA9794D342D260035A70E365224C66E;
struct TypeData_t01D670B4E71B5571B38C7412B1E652A47D6AF66A;

IL2CPP_EXTERN_C RuntimeClass* Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* StringBuilder_t_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1;
IL2CPP_EXTERN_C String_t* _stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745;
IL2CPP_EXTERN_C String_t* _stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146;
IL2CPP_EXTERN_C String_t* _stringLiteral34761125773C5996C26093DBBA81463705909069;
IL2CPP_EXTERN_C String_t* _stringLiteral3DCC6243286938BE75C3FA773B9BA71160A2E869;
IL2CPP_EXTERN_C String_t* _stringLiteral491788442E76F5D7830F0DBFCF8EDD98854F636F;
IL2CPP_EXTERN_C const RuntimeMethod* BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_RuntimeMethod_var;
struct CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_com;
struct CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_pinvoke;
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_com;
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_pinvoke;
struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;
struct PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com;
struct PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke;

struct FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F;
struct SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C;
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402  : public RuntimeObject
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___fieldsContainer;
	VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* ___U3CU3E4__this;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* ___componentFields;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CU3E9__1;
};
struct U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45  : public RuntimeObject
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___fieldsContainer;
	VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* ___U3CU3E4__this;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* ___componentFields;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CU3E9__1;
};
struct U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807  : public RuntimeObject
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___fieldsContainer;
	VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* ___U3CU3E4__this;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* ___componentFields;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CU3E9__1;
};
struct U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553  : public RuntimeObject
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___fieldsContainer;
	VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* ___U3CU3E4__this;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* ___componentFields;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CU3E9__1;
};
struct U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE  : public RuntimeObject
{
	int32_t ___componentIndex;
	VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* ___U3CU3E4__this;
};
struct U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00  : public RuntimeObject
{
	int32_t ___componentIndex;
	VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* ___U3CU3E4__this;
};
struct U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5  : public RuntimeObject
{
	int32_t ___componentIndex;
	VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* ___U3CU3E4__this;
};
struct U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6  : public RuntimeObject
{
	int32_t ___componentIndex;
	VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* ___U3CU3E4__this;
};
struct CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4  : public RuntimeObject
{
	bool ___isIMGUIContainer;
	EventCallbackRegistry_tE18297C3F7E535BD82EDA83EC6D6DAA386226B85* ___m_CallbackRegistry;
};
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0  : public RuntimeObject
{
	bool ___m_isReadOnly;
	int32_t ___cultureID;
	int32_t ___parent_lcid;
	int32_t ___datetime_index;
	int32_t ___number_index;
	int32_t ___default_calendar_type;
	bool ___m_useUserOverride;
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___numInfo;
	DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A* ___dateTimeInfo;
	TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4* ___textInfo;
	String_t* ___m_name;
	String_t* ___englishname;
	String_t* ___nativename;
	String_t* ___iso3lang;
	String_t* ___iso2lang;
	String_t* ___win3lang;
	String_t* ___territory;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___native_calendar_names;
	CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57* ___compareInfo;
	void* ___textinfo_data;
	int32_t ___m_dataItem;
	Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B* ___calendar;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___parent_culture;
	bool ___constructed;
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___cached_serialized_form;
	CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D* ___m_cultureData;
	bool ___m_isInherited;
};
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_pinvoke
{
	int32_t ___m_isReadOnly;
	int32_t ___cultureID;
	int32_t ___parent_lcid;
	int32_t ___datetime_index;
	int32_t ___number_index;
	int32_t ___default_calendar_type;
	int32_t ___m_useUserOverride;
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___numInfo;
	DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A* ___dateTimeInfo;
	TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4* ___textInfo;
	char* ___m_name;
	char* ___englishname;
	char* ___nativename;
	char* ___iso3lang;
	char* ___iso2lang;
	char* ___win3lang;
	char* ___territory;
	char** ___native_calendar_names;
	CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57* ___compareInfo;
	void* ___textinfo_data;
	int32_t ___m_dataItem;
	Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B* ___calendar;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_pinvoke* ___parent_culture;
	int32_t ___constructed;
	Il2CppSafeArray* ___cached_serialized_form;
	CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_pinvoke* ___m_cultureData;
	int32_t ___m_isInherited;
};
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_com
{
	int32_t ___m_isReadOnly;
	int32_t ___cultureID;
	int32_t ___parent_lcid;
	int32_t ___datetime_index;
	int32_t ___number_index;
	int32_t ___default_calendar_type;
	int32_t ___m_useUserOverride;
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___numInfo;
	DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A* ___dateTimeInfo;
	TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4* ___textInfo;
	Il2CppChar* ___m_name;
	Il2CppChar* ___englishname;
	Il2CppChar* ___nativename;
	Il2CppChar* ___iso3lang;
	Il2CppChar* ___iso2lang;
	Il2CppChar* ___win3lang;
	Il2CppChar* ___territory;
	Il2CppChar** ___native_calendar_names;
	CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57* ___compareInfo;
	void* ___textinfo_data;
	int32_t ___m_dataItem;
	Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B* ___calendar;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_com* ___parent_culture;
	int32_t ___constructed;
	Il2CppSafeArray* ___cached_serialized_form;
	CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_com* ___m_cultureData;
	int32_t ___m_isInherited;
};
struct MemberInfo_t  : public RuntimeObject
{
};
struct NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472  : public RuntimeObject
{
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ___numberGroupSizes;
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ___currencyGroupSizes;
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ___percentGroupSizes;
	String_t* ___positiveSign;
	String_t* ___negativeSign;
	String_t* ___numberDecimalSeparator;
	String_t* ___numberGroupSeparator;
	String_t* ___currencyGroupSeparator;
	String_t* ___currencyDecimalSeparator;
	String_t* ___currencySymbol;
	String_t* ___ansiCurrencySymbol;
	String_t* ___nanSymbol;
	String_t* ___positiveInfinitySymbol;
	String_t* ___negativeInfinitySymbol;
	String_t* ___percentDecimalSeparator;
	String_t* ___percentGroupSeparator;
	String_t* ___percentSymbol;
	String_t* ___perMilleSymbol;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___nativeDigits;
	int32_t ___m_dataItem;
	int32_t ___numberDecimalDigits;
	int32_t ___currencyDecimalDigits;
	int32_t ___currencyPositivePattern;
	int32_t ___currencyNegativePattern;
	int32_t ___numberNegativePattern;
	int32_t ___percentPositivePattern;
	int32_t ___percentNegativePattern;
	int32_t ___percentDecimalDigits;
	int32_t ___digitSubstitution;
	bool ___isReadOnly;
	bool ___m_useUserOverride;
	bool ___m_isInvariant;
	bool ___validForParseAsNumber;
	bool ___validForParseAsCurrency;
};
struct String_t  : public RuntimeObject
{
	int32_t ____stringLength;
	Il2CppChar ____firstChar;
};
struct StringBuilder_t  : public RuntimeObject
{
	CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ___m_ChunkChars;
	StringBuilder_t* ___m_ChunkPrevious;
	int32_t ___m_ChunkLength;
	int32_t ___m_ChunkOffset;
	int32_t ___m_MaxCapacity;
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct CustomStyleProperty_1_tE4B20CAB5BCFEE711EB4A26F077DC700987C0C2D 
{
	String_t* ___U3CnameU3Ek__BackingField;
};
#ifndef CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_pinvoke_define
#define CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_pinvoke_define
struct CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_pinvoke
{
	char* ___U3CnameU3Ek__BackingField;
};
#endif
#ifndef CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_com_define
#define CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_com_define
struct CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_com
{
	Il2CppChar* ___U3CnameU3Ek__BackingField;
};
#endif
struct CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 
{
	String_t* ___U3CnameU3Ek__BackingField;
};
#ifndef CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_pinvoke_define
#define CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_pinvoke_define
struct CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_pinvoke
{
	char* ___U3CnameU3Ek__BackingField;
};
#endif
#ifndef CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_com_define
#define CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_com_define
struct CustomStyleProperty_1_t8315EF5D1C5F5FB5F920B77E40695C07DAAB349A_marshaled_com
{
	Il2CppChar* ___U3CnameU3Ek__BackingField;
};
#endif
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct Byte_t94D9231AC217BE4D2E004C4CD32DF6D099EA41A3 
{
	uint8_t ___m_value;
};
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17 
{
	Il2CppChar ___m_value;
};
struct Double_tE150EF3D1D43DEE85D533810AB4C742307EEDE5F 
{
	double ___m_value;
};
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2  : public ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F
{
};
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_marshaled_pinvoke
{
};
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_marshaled_com
{
};
struct Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0  : public CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4
{
	bool ___m_Focusable;
	int32_t ___m_TabIndex;
	bool ___m_DelegatesFocus;
	bool ___m_ExcludeFromFocusRing;
	bool ___U3CisEligibleToReceiveFocusFromDisabledChildU3Ek__BackingField;
};
struct Int16_tB8EF286A9C33492FA6E6D6E67320BE93E794A175 
{
	int16_t ___m_value;
};
struct Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C 
{
	int32_t ___m_value;
};
struct Int64_t092CFB123BE63C28ACDAF65C68F21A526050DBA3 
{
	int64_t ___m_value;
};
struct IntPtr_t 
{
	void* ___m_value;
};
struct PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 
{
	int32_t ___id;
};
struct Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D 
{
	float ___m_XMin;
	float ___m_YMin;
	float ___m_Width;
	float ___m_Height;
};
struct Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A 
{
	union
	{
		#pragma pack(push, tp, 1)
		struct
		{
			uint8_t ___byte_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint8_t ___byte_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_1_OffsetPadding[1];
			uint8_t ___byte_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_1_OffsetPadding_forAlignmentOnly[1];
			uint8_t ___byte_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_2_OffsetPadding[2];
			uint8_t ___byte_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_2_OffsetPadding_forAlignmentOnly[2];
			uint8_t ___byte_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_3_OffsetPadding[3];
			uint8_t ___byte_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_3_OffsetPadding_forAlignmentOnly[3];
			uint8_t ___byte_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_4_OffsetPadding[4];
			uint8_t ___byte_4;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_4_OffsetPadding_forAlignmentOnly[4];
			uint8_t ___byte_4_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_5_OffsetPadding[5];
			uint8_t ___byte_5;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_5_OffsetPadding_forAlignmentOnly[5];
			uint8_t ___byte_5_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_6_OffsetPadding[6];
			uint8_t ___byte_6;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_6_OffsetPadding_forAlignmentOnly[6];
			uint8_t ___byte_6_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_7_OffsetPadding[7];
			uint8_t ___byte_7;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_7_OffsetPadding_forAlignmentOnly[7];
			uint8_t ___byte_7_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_8_OffsetPadding[8];
			uint8_t ___byte_8;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_8_OffsetPadding_forAlignmentOnly[8];
			uint8_t ___byte_8_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_9_OffsetPadding[9];
			uint8_t ___byte_9;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_9_OffsetPadding_forAlignmentOnly[9];
			uint8_t ___byte_9_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_10_OffsetPadding[10];
			uint8_t ___byte_10;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_10_OffsetPadding_forAlignmentOnly[10];
			uint8_t ___byte_10_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_11_OffsetPadding[11];
			uint8_t ___byte_11;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_11_OffsetPadding_forAlignmentOnly[11];
			uint8_t ___byte_11_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_12_OffsetPadding[12];
			uint8_t ___byte_12;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_12_OffsetPadding_forAlignmentOnly[12];
			uint8_t ___byte_12_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_13_OffsetPadding[13];
			uint8_t ___byte_13;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_13_OffsetPadding_forAlignmentOnly[13];
			uint8_t ___byte_13_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_14_OffsetPadding[14];
			uint8_t ___byte_14;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_14_OffsetPadding_forAlignmentOnly[14];
			uint8_t ___byte_14_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___byte_15_OffsetPadding[15];
			uint8_t ___byte_15;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___byte_15_OffsetPadding_forAlignmentOnly[15];
			uint8_t ___byte_15_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			int8_t ___sbyte_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			int8_t ___sbyte_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_1_OffsetPadding[1];
			int8_t ___sbyte_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_1_OffsetPadding_forAlignmentOnly[1];
			int8_t ___sbyte_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_2_OffsetPadding[2];
			int8_t ___sbyte_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_2_OffsetPadding_forAlignmentOnly[2];
			int8_t ___sbyte_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_3_OffsetPadding[3];
			int8_t ___sbyte_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_3_OffsetPadding_forAlignmentOnly[3];
			int8_t ___sbyte_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_4_OffsetPadding[4];
			int8_t ___sbyte_4;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_4_OffsetPadding_forAlignmentOnly[4];
			int8_t ___sbyte_4_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_5_OffsetPadding[5];
			int8_t ___sbyte_5;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_5_OffsetPadding_forAlignmentOnly[5];
			int8_t ___sbyte_5_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_6_OffsetPadding[6];
			int8_t ___sbyte_6;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_6_OffsetPadding_forAlignmentOnly[6];
			int8_t ___sbyte_6_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_7_OffsetPadding[7];
			int8_t ___sbyte_7;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_7_OffsetPadding_forAlignmentOnly[7];
			int8_t ___sbyte_7_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_8_OffsetPadding[8];
			int8_t ___sbyte_8;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_8_OffsetPadding_forAlignmentOnly[8];
			int8_t ___sbyte_8_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_9_OffsetPadding[9];
			int8_t ___sbyte_9;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_9_OffsetPadding_forAlignmentOnly[9];
			int8_t ___sbyte_9_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_10_OffsetPadding[10];
			int8_t ___sbyte_10;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_10_OffsetPadding_forAlignmentOnly[10];
			int8_t ___sbyte_10_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_11_OffsetPadding[11];
			int8_t ___sbyte_11;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_11_OffsetPadding_forAlignmentOnly[11];
			int8_t ___sbyte_11_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_12_OffsetPadding[12];
			int8_t ___sbyte_12;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_12_OffsetPadding_forAlignmentOnly[12];
			int8_t ___sbyte_12_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_13_OffsetPadding[13];
			int8_t ___sbyte_13;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_13_OffsetPadding_forAlignmentOnly[13];
			int8_t ___sbyte_13_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_14_OffsetPadding[14];
			int8_t ___sbyte_14;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_14_OffsetPadding_forAlignmentOnly[14];
			int8_t ___sbyte_14_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___sbyte_15_OffsetPadding[15];
			int8_t ___sbyte_15;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___sbyte_15_OffsetPadding_forAlignmentOnly[15];
			int8_t ___sbyte_15_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			uint16_t ___uint16_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint16_t ___uint16_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_1_OffsetPadding[2];
			uint16_t ___uint16_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_1_OffsetPadding_forAlignmentOnly[2];
			uint16_t ___uint16_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_2_OffsetPadding[4];
			uint16_t ___uint16_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_2_OffsetPadding_forAlignmentOnly[4];
			uint16_t ___uint16_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_3_OffsetPadding[6];
			uint16_t ___uint16_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_3_OffsetPadding_forAlignmentOnly[6];
			uint16_t ___uint16_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_4_OffsetPadding[8];
			uint16_t ___uint16_4;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_4_OffsetPadding_forAlignmentOnly[8];
			uint16_t ___uint16_4_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_5_OffsetPadding[10];
			uint16_t ___uint16_5;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_5_OffsetPadding_forAlignmentOnly[10];
			uint16_t ___uint16_5_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_6_OffsetPadding[12];
			uint16_t ___uint16_6;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_6_OffsetPadding_forAlignmentOnly[12];
			uint16_t ___uint16_6_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint16_7_OffsetPadding[14];
			uint16_t ___uint16_7;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint16_7_OffsetPadding_forAlignmentOnly[14];
			uint16_t ___uint16_7_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			int16_t ___int16_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			int16_t ___int16_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_1_OffsetPadding[2];
			int16_t ___int16_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_1_OffsetPadding_forAlignmentOnly[2];
			int16_t ___int16_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_2_OffsetPadding[4];
			int16_t ___int16_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_2_OffsetPadding_forAlignmentOnly[4];
			int16_t ___int16_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_3_OffsetPadding[6];
			int16_t ___int16_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_3_OffsetPadding_forAlignmentOnly[6];
			int16_t ___int16_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_4_OffsetPadding[8];
			int16_t ___int16_4;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_4_OffsetPadding_forAlignmentOnly[8];
			int16_t ___int16_4_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_5_OffsetPadding[10];
			int16_t ___int16_5;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_5_OffsetPadding_forAlignmentOnly[10];
			int16_t ___int16_5_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_6_OffsetPadding[12];
			int16_t ___int16_6;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_6_OffsetPadding_forAlignmentOnly[12];
			int16_t ___int16_6_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int16_7_OffsetPadding[14];
			int16_t ___int16_7;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int16_7_OffsetPadding_forAlignmentOnly[14];
			int16_t ___int16_7_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			uint32_t ___uint32_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint32_t ___uint32_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint32_1_OffsetPadding[4];
			uint32_t ___uint32_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint32_1_OffsetPadding_forAlignmentOnly[4];
			uint32_t ___uint32_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint32_2_OffsetPadding[8];
			uint32_t ___uint32_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint32_2_OffsetPadding_forAlignmentOnly[8];
			uint32_t ___uint32_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint32_3_OffsetPadding[12];
			uint32_t ___uint32_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint32_3_OffsetPadding_forAlignmentOnly[12];
			uint32_t ___uint32_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			int32_t ___int32_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			int32_t ___int32_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int32_1_OffsetPadding[4];
			int32_t ___int32_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int32_1_OffsetPadding_forAlignmentOnly[4];
			int32_t ___int32_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int32_2_OffsetPadding[8];
			int32_t ___int32_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int32_2_OffsetPadding_forAlignmentOnly[8];
			int32_t ___int32_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int32_3_OffsetPadding[12];
			int32_t ___int32_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int32_3_OffsetPadding_forAlignmentOnly[12];
			int32_t ___int32_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			uint64_t ___uint64_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			uint64_t ___uint64_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___uint64_1_OffsetPadding[8];
			uint64_t ___uint64_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___uint64_1_OffsetPadding_forAlignmentOnly[8];
			uint64_t ___uint64_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			int64_t ___int64_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			int64_t ___int64_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___int64_1_OffsetPadding[8];
			int64_t ___int64_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___int64_1_OffsetPadding_forAlignmentOnly[8];
			int64_t ___int64_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			float ___single_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			float ___single_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___single_1_OffsetPadding[4];
			float ___single_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___single_1_OffsetPadding_forAlignmentOnly[4];
			float ___single_1_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___single_2_OffsetPadding[8];
			float ___single_2;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___single_2_OffsetPadding_forAlignmentOnly[8];
			float ___single_2_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___single_3_OffsetPadding[12];
			float ___single_3;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___single_3_OffsetPadding_forAlignmentOnly[12];
			float ___single_3_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			double ___double_0;
		};
		#pragma pack(pop, tp)
		struct
		{
			double ___double_0_forAlignmentOnly;
		};
		#pragma pack(push, tp, 1)
		struct
		{
			char ___double_1_OffsetPadding[8];
			double ___double_1;
		};
		#pragma pack(pop, tp)
		struct
		{
			char ___double_1_OffsetPadding_forAlignmentOnly[8];
			double ___double_1_forAlignmentOnly;
		};
	};
};
struct SByte_tFEFFEF5D2FEBF5207950AE6FAC150FC53B668DB5 
{
	int8_t ___m_value;
};
struct Single_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C 
{
	float ___m_value;
};
struct UInt16_tF4C148C876015C212FD72652D0B6ED8CC247A455 
{
	uint16_t ___m_value;
};
struct UInt32_t1833D51FFA667B18A5AA4B8D34DE284F8495D29B 
{
	uint32_t ___m_value;
};
struct UInt64_t8F12534CC8FC4B5860F2A2CD1EE79D322E7A41AF 
{
	uint64_t ___m_value;
};
struct UnmanagedDataHandle_t5295F32E122AF2E09BF729381A22BD86B72C1DD1 
{
	int32_t ___Index;
	int32_t ___Version;
};
struct Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 
{
	float ___x;
	float ___y;
};
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 
{
	float ___x;
	float ___y;
	float ___z;
};
struct Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 
{
	float ___x;
	float ___y;
	float ___z;
	float ___w;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct Hierarchy_t4CF226F0EDE9C117C51C505730FC80641B1F1677 
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_Owner;
};
struct Hierarchy_t4CF226F0EDE9C117C51C505730FC80641B1F1677_marshaled_pinvoke
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_Owner;
};
struct Hierarchy_t4CF226F0EDE9C117C51C505730FC80641B1F1677_marshaled_com
{
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_Owner;
};
struct Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 
{
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A ___register;
};
struct Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A 
{
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A ___register;
};
struct Vector_1_t4FB40153F5AFF7BFDFB20E1BCB98343E42252AD2 
{
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A ___register;
};
struct Allocator_t996642592271AAD9EE688F142741D512C07B5824 
{
	int32_t ___value__;
};
struct Delegate_t  : public RuntimeObject
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	RuntimeObject* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	bool ___method_is_virtual;
};
struct Delegate_t_marshaled_pinvoke
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	Il2CppIUnknown* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	int32_t ___method_is_virtual;
};
struct Delegate_t_marshaled_com
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	Il2CppIUnknown* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	int32_t ___method_is_virtual;
};
struct DispatchMode_t19462FC6D47F5BF63B55FBDEB5C7D498D50DB3DC 
{
	int32_t ___value__;
};
struct Exception_t  : public RuntimeObject
{
	String_t* ____className;
	String_t* ____message;
	RuntimeObject* ____data;
	Exception_t* ____innerException;
	String_t* ____helpURL;
	RuntimeObject* ____stackTrace;
	String_t* ____stackTraceString;
	String_t* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	RuntimeObject* ____dynamicMethods;
	int32_t ____HResult;
	String_t* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Exception_t_marshaled_pinvoke
{
	char* ____className;
	char* ____message;
	RuntimeObject* ____data;
	Exception_t_marshaled_pinvoke* ____innerException;
	char* ____helpURL;
	Il2CppIUnknown* ____stackTrace;
	char* ____stackTraceString;
	char* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	Il2CppIUnknown* ____dynamicMethods;
	int32_t ____HResult;
	char* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	Il2CppSafeArray* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className;
	Il2CppChar* ____message;
	RuntimeObject* ____data;
	Exception_t_marshaled_com* ____innerException;
	Il2CppChar* ____helpURL;
	Il2CppIUnknown* ____stackTrace;
	Il2CppChar* ____stackTraceString;
	Il2CppChar* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	Il2CppIUnknown* ____dynamicMethods;
	int32_t ____HResult;
	Il2CppChar* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	Il2CppSafeArray* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct FlexDirection_t0B086E97C9B1B8F1D94B51CBEAE6564B2A7C15DC 
{
	int32_t ___value__;
};
struct LanguageDirection_t30A3B6BBCEE6A6F57641E4E008E0DCC40603558C 
{
	int32_t ___value__;
};
struct ProfilerMarker_tA256E18DA86EDBC5528CE066FC91C96EE86501AD 
{
	intptr_t ___m_Ptr;
};
struct PropagationPhase_tF3BE8BF5ED45FC52A828B7B6F078B64F01FAE6D6 
{
	int32_t ___value__;
};
struct PropertyPathPartKind_t82152825D88A0E450DDCE8503272A10595047F87 
{
	int32_t ___value__;
};
struct PseudoStates_tF4AB056E8743741BCE464A0983A060A53AAB7E4D 
{
	int32_t ___value__;
};
struct RenderHints_t4032FC4AB3FD946FD2A484865B8861730D9035E7 
{
	int32_t ___value__;
};
struct RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B 
{
	intptr_t ___value;
};
struct StyleKeyword_t2812E72266C15CBA8927586972DC2FD27B10E705 
{
	int32_t ___value__;
};
struct TrickleDown_t862EABE449B6C94F79F2B1D713990C35ABADDF38 
{
	int32_t ___value__;
};
struct VisualElementFlags_t4D1066E11400967A1A2DA7331391ACDC4AA14409 
{
	int32_t ___value__;
};
struct Context_tAF49F3B4A4D21F81E0AB6F2B4315011FAD804FE4 
{
	int32_t ___value__;
};
struct Flags_tBBD3C554E9057BB9AC0476F92D0328575F2C4193 
{
	int32_t ___value__;
};
struct EventPropagation_t024AF56F7A787C03AA21B065B624553EF52E7B83 
{
	int32_t ___value__;
};
struct LifeCycleStatus_tEE500629F5431B574B8047EB70864747D348D38C 
{
	int32_t ___value__;
};
struct StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE 
{
	int32_t ___m_Value;
	int32_t ___m_Keyword;
};
struct StyleEnum_1_tAFD4AA79F79D19DE23805D46DA67778A70EBBF26 
{
	int32_t ___m_Value;
	int32_t ___m_Keyword;
};
struct VectorSizeHelper_tC26CEAD1B0D88F765A24D653A74900C4C7FEBD18 
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ____placeholder;
	uint8_t ____byte;
};
struct VectorSizeHelper_tF54ACCE947CB8A38047BEB642392A4E7345A157D 
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ____placeholder;
	uint8_t ____byte;
};
struct VectorSizeHelper_tAB183E4966E0B97A19A75D213EDAE51037A6BF3C 
{
	Vector_1_t4FB40153F5AFF7BFDFB20E1BCB98343E42252AD2 ____placeholder;
	uint8_t ____byte;
};
struct EventBase_tD7F89B936EB8074AE31E7B15976C072277371F7C  : public RuntimeObject
{
	int32_t ___U3CeventCategoriesU3Ek__BackingField;
	int64_t ___U3CtimestampU3Ek__BackingField;
	uint64_t ___U3CeventIdU3Ek__BackingField;
	uint64_t ___U3CtriggerEventIdU3Ek__BackingField;
	int32_t ___U3CpropagationU3Ek__BackingField;
	int32_t ___U3ClifeCycleStatusU3Ek__BackingField;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___U3CelementTargetU3Ek__BackingField;
	int32_t ___U3CpropagationPhaseU3Ek__BackingField;
	RuntimeObject* ___m_CurrentTarget;
	Event_tEBC6F24B56CE22B9C9AD1AC6C24A6B83BC3860CB* ___m_ImguiEvent;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___U3CoriginalMousePositionU3Ek__BackingField;
};
struct MemoryLabel_t29CE7AB312D2ED888B444BBE5D452F7132EC9DB2 
{
	union
	{
		struct
		{
			intptr_t ___pointer;
			int32_t ___allocator;
		};
		uint8_t MemoryLabel_t29CE7AB312D2ED888B444BBE5D452F7132EC9DB2__padding[16];
	};
};
struct MulticastDelegate_t  : public Delegate_t
{
	DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771* ___delegates;
};
struct MulticastDelegate_t_marshaled_pinvoke : public Delegate_t_marshaled_pinvoke
{
	Delegate_t_marshaled_pinvoke** ___delegates;
};
struct MulticastDelegate_t_marshaled_com : public Delegate_t_marshaled_com
{
	Delegate_t_marshaled_com** ___delegates;
};
struct PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF 
{
	int32_t ___m_Kind;
	String_t* ___m_Name;
	int32_t ___m_Index;
	RuntimeObject* ___m_Key;
};
struct PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke
{
	int32_t ___m_Kind;
	char* ___m_Name;
	int32_t ___m_Index;
	Il2CppIUnknown* ___m_Key;
};
struct PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com
{
	int32_t ___m_Kind;
	Il2CppChar* ___m_Name;
	int32_t ___m_Index;
	Il2CppIUnknown* ___m_Key;
};
struct SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295  : public Exception_t
{
};
struct Type_t  : public MemberInfo_t
{
	RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B ____impl;
};
struct Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF  : public RuntimeObject
{
	int32_t ___U3CorderU3Ek__BackingField;
	Panel_t3A0D2006E8AEA607A6DF5188138E463A26085295* ___m_Panel;
	bool ___U3CkeepLayoutWhenHiddenU3Ek__BackingField;
	bool ___m_IsHiddenBySearchFilter;
	String_t* ___m_AdditionalSearchText;
	bool ___m_RequiresLegacyStateHandling;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_VisualElement;
	int32_t ___m_Context;
	RuntimeObject* ___m_Parent;
	int32_t ___U3CflagsU3Ek__BackingField;
	String_t* ___U3CdisplayNameU3Ek__BackingField;
	String_t* ___U3CtooltipU3Ek__BackingField;
	String_t* ___U3CqueryPathU3Ek__BackingField;
	Func_1_t2BE7F58348C9CC544A8973B3A9E55541DE43C457* ___isHiddenCallback;
};
struct Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C  : public MulticastDelegate_t
{
};
struct Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A  : public MulticastDelegate_t
{
};
struct EventBase_1_t01A865DB5F1B17F837A1670705B7579323F76216  : public EventBase_tD7F89B936EB8074AE31E7B15976C072277371F7C
{
	int32_t ___m_RefCount;
};
struct EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07  : public MulticastDelegate_t
{
};
struct Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED  : public Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF
{
	Func_1_t9EB8CE9DFD9B703BC79F2087B16EA394B7A9F9A1* ___U3CgetterU3Ek__BackingField;
	Action_1_t17E52B12DC24FA6C9DD52F87043C85BEA889BB81* ___U3CsetterU3Ek__BackingField;
	Action_2_tF46B14C98A24F40F2279A1D4296BB9078938C034* ___onValueChanged;
};
struct Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18  : public Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF
{
	Func_1_t58C51DB29153B53A9136AE397958F3FCC1F596EC* ___U3CgetterU3Ek__BackingField;
	Action_1_t2EDB30EAB747FDF563DD6410FC76AF861A09A0C2* ___U3CsetterU3Ek__BackingField;
	Action_2_t115BA48255E00E3E7D79535060D729C4822CAFF3* ___onValueChanged;
};
struct Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD  : public Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF
{
	Func_1_t704C051013549CDD77A31AEC405EA270221633B3* ___U3CgetterU3Ek__BackingField;
	Action_1_t84D0CA347FC997E1202ECA3ED828B057841444EF* ___U3CsetterU3Ek__BackingField;
	Action_2_t302322518DED0A32BC10F069AAEE117BC9C20917* ___onValueChanged;
};
struct Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258  : public Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF
{
	Func_1_t87EB6A475C10479F9DA4442B05AC1022C1B7419C* ___U3CgetterU3Ek__BackingField;
	Action_1_tC8822DDEF41267DA3844DAD787ACE63C0C385E89* ___U3CsetterU3Ek__BackingField;
	Action_2_t09DA61027B1820298B3AEBAB627FBF4C0CCC66B4* ___onValueChanged;
};
struct Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780  : public MulticastDelegate_t
{
};
struct Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7  : public MulticastDelegate_t
{
};
struct IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82  : public SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295
{
};
struct PropertyPath_tA523CA2740853534DF6C009C588464B45A6D0A79 
{
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF ___m_Part0;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF ___m_Part1;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF ___m_Part2;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF ___m_Part3;
	PropertyPathPartU5BU5D_t7994D542F14DDDDEABB1792C335C20149399AEBB* ___m_AdditionalParts;
	int32_t ___U3CLengthU3Ek__BackingField;
};
struct PropertyPath_tA523CA2740853534DF6C009C588464B45A6D0A79_marshaled_pinvoke
{
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke ___m_Part0;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke ___m_Part1;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke ___m_Part2;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke ___m_Part3;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_pinvoke* ___m_AdditionalParts;
	int32_t ___U3CLengthU3Ek__BackingField;
};
struct PropertyPath_tA523CA2740853534DF6C009C588464B45A6D0A79_marshaled_com
{
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com ___m_Part0;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com ___m_Part1;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com ___m_Part2;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com ___m_Part3;
	PropertyPathPart_tFB308743948D2298957DC1898D90AF2ACFED9DFF_marshaled_com* ___m_AdditionalParts;
	int32_t ___U3CLengthU3Ek__BackingField;
};
struct UnmanagedDataStore_t66CFDF2DBB3C86F8A58F5B3EBEE5E9537BDF2759 
{
	MemoryLabel_t29CE7AB312D2ED888B444BBE5D452F7132EC9DB2 ___m_MemoryLabel;
	Data_t6BD087CC0FA9794D342D260035A70E365224C66E* ___m_Data;
};
struct FocusEventBase_1_t1F27FAA60011E82AD2DB0EA212CE4C0D7BF7B022  : public EventBase_1_t01A865DB5F1B17F837A1670705B7579323F76216
{
	Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0* ___U3CrelatedTargetU3Ek__BackingField;
	FocusChangeDirection_t95F4784CF68331792EB9077AEAB01E17822991EF* ___U3CdirectionU3Ek__BackingField;
	FocusController_t5D2E45F2CCBE3B7082DE4088EE03C2E8F736011A* ___U3CfocusControllerU3Ek__BackingField;
	bool ___U3CIsFocusDelegatedU3Ek__BackingField;
};
struct VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87  : public Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED
{
	float ___incStep;
	float ___incStepMult;
	int32_t ___decimals;
	int32_t ___selectedComponent;
};
struct VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D  : public Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18
{
	float ___incStep;
	float ___incStepMult;
	int32_t ___decimals;
	int32_t ___selectedComponent;
};
struct VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3  : public Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD
{
	float ___incStep;
	float ___incStepMult;
	int32_t ___decimals;
	int32_t ___selectedComponent;
};
struct VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F  : public Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258
{
	float ___incStep;
	float ___incStepMult;
	int32_t ___decimals;
	int32_t ___selectedComponent;
};
struct BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E 
{
	PropertyPath_tA523CA2740853534DF6C009C588464B45A6D0A79 ___m_PropertyPath;
	String_t* ___m_Path;
};
struct BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E_marshaled_pinvoke
{
	PropertyPath_tA523CA2740853534DF6C009C588464B45A6D0A79_marshaled_pinvoke ___m_PropertyPath;
	char* ___m_Path;
};
struct BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E_marshaled_com
{
	PropertyPath_tA523CA2740853534DF6C009C588464B45A6D0A79_marshaled_com ___m_PropertyPath;
	Il2CppChar* ___m_Path;
};
struct LayoutDataAccess_t99AA56349D0BC76F3742B927F4F0DDB21511FBBA 
{
	int32_t ___m_Manager;
	UnmanagedDataStore_t66CFDF2DBB3C86F8A58F5B3EBEE5E9537BDF2759 ___m_Nodes;
	UnmanagedDataStore_t66CFDF2DBB3C86F8A58F5B3EBEE5E9537BDF2759 ___m_Configs;
};
struct FocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE  : public FocusEventBase_1_t1F27FAA60011E82AD2DB0EA212CE4C0D7BF7B022
{
};
struct LayoutNode_tADF081B0F16F76B66459DE38F3AD8EC098F22CBE 
{
	LayoutDataAccess_t99AA56349D0BC76F3742B927F4F0DDB21511FBBA ___m_Access;
	UnmanagedDataHandle_t5295F32E122AF2E09BF729381A22BD86B72C1DD1 ___m_Handle;
};
struct VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115  : public Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0
{
	int32_t ___U3CUnityEngine_UIElements_IStylePropertyAnimations_runningAnimationCountU3Ek__BackingField;
	int32_t ___U3CUnityEngine_UIElements_IStylePropertyAnimations_completedAnimationCountU3Ek__BackingField;
	String_t* ___m_Name;
	List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* ___m_ClassList;
	Dictionary_2_tBCCCFBCAC02A3C03E3C84D75696D4860D7444A35* ___m_PropertyBag;
	int32_t ___m_Flags;
	String_t* ___m_ViewDataKey;
	int32_t ___m_RenderHints;
	Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D ___lastLayout;
	Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D ___lastPseudoPadding;
	RenderData_t1ABE116B2B5E0409AC699E195922516606531DC2* ___renderData;
	RenderData_t1ABE116B2B5E0409AC699E195922516606531DC2* ___nestedRenderData;
	int32_t ___insertionIndex;
	int32_t ___triggerPseudoMask;
	int32_t ___dependencyPseudoMask;
	int32_t ___m_PseudoStates;
	int32_t ___U3CcontainedPointerIdsU3Ek__BackingField;
	LayoutNode_tADF081B0F16F76B66459DE38F3AD8EC098F22CBE ___m_LayoutNode;
	VisualElementTransformData_t3DD575B5990B68FF956673EFF036171C86A38DF3* ___m_TransformDataPTr;
	StyleVariableContext_tF74F2787CE1F6BEBBFBFF0771CF493AC9E403527* ___variableContext;
	int32_t ___inheritedStylesHash;
	uint32_t ___controlid;
	int32_t ___imguiContainerDescendantCount;
	bool ___m_EnabledSelf;
	int32_t ___m_LanguageDirection;
	int32_t ___m_LocalLanguageDirection;
	Action_1_t3DC3411926243F1DB9C330F8E105B904E38C1A0B* ___U3CgenerateVisualContentU3Ek__BackingField;
	List_1_t96E9133B70FB6765E6B138E810D33E18901715DA* ___m_RunningAnimations;
	RuntimeObject* ___m_DataSource;
	PathRef_t76F7677792A14AF9D6A6EAB7F08C1A3DC2B27A55* ___m_DataSourcePath;
	List_1_t592DF2E59D6E244DA0041698383484A07C7E0480* ___m_Bindings;
	Type_t* ___U3CdataSourceTypeU3Ek__BackingField;
	int32_t ___m_TrickleDownHandleEventCategories;
	int32_t ___m_BubbleUpHandleEventCategories;
	int32_t ___m_BubbleUpEventCallbackCategories;
	int32_t ___m_TrickleDownEventCallbackCategories;
	int32_t ___m_EventInterestSelfCategories;
	int32_t ___m_CachedEventInterestParentCategories;
	uint32_t ___m_NextParentCachedVersion;
	uint32_t ___m_NextParentRequiredVersion;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedNextParentWithEventInterests;
	Hierarchy_t4CF226F0EDE9C117C51C505730FC80641B1F1677 ___U3ChierarchyU3Ek__BackingField;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_PhysicalParent;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_LogicalParent;
	List_1_t6115BBE78FE9310B180A2027321DF46F2A06AC95* ___m_Children;
	BaseVisualElementPanel_tE3811F3D1474B72CB6CD5BCEECFF5B5CBEC1E303* ___U3CelementPanelU3Ek__BackingField;
	VisualTreeAsset_tFB5BF81F0780A412AE5A7C2C552B3EEA64EA2EEB* ___m_VisualTreeAssetSource;
	InlineStyleAccess_t5CA7877999C9442491A220AE50D605C84D09A165* ___inlineStyleAccess;
	ResolvedStyleAccess_t226CC840EBACEE31CE1139ED5F717532AFFAEB45* ___resolvedStyleAccess;
	List_1_tEA16F82F7871418E28EB6F551D77A8AD9F2E337F* ___styleSheetList;
	TypeData_t01D670B4E71B5571B38C7412B1E652A47D6AF66A* ___m_TypeData;
};
struct BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C  : public VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115
{
	RuntimeObject* ___U3CbindingU3Ek__BackingField;
	String_t* ___U3CbindingPathU3Ek__BackingField;
};
struct BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7  : public BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C
{
	float ___m_LabelWidthRatio;
	float ___m_LabelExtraPadding;
	float ___m_LabelBaseMinWidth;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_VisualInput;
	Action_1_tAE453A6458E1DAD68CAF44DC8FF62EC18FEAA930* ___expressionEvaluated;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___viewDataRestored;
	float ___m_Value;
	Func_2_t2A7432CC4F64D0DF6D8629208B154CF139B39AF2* ___onValidateValue;
	int32_t ___U3CdispatchModeU3Ek__BackingField;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___U3ClabelElementU3Ek__BackingField;
	bool ___m_ShowMixedValue;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___m_MixedValueLabel;
	bool ___m_SkipValidation;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedContextWidthElement;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedInspectorElement;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingStartedU3Ek__BackingField;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingEndedU3Ek__BackingField;
};
struct BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507  : public BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C
{
	float ___m_LabelWidthRatio;
	float ___m_LabelExtraPadding;
	float ___m_LabelBaseMinWidth;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_VisualInput;
	Action_1_tAE453A6458E1DAD68CAF44DC8FF62EC18FEAA930* ___expressionEvaluated;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___viewDataRestored;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___m_Value;
	Func_2_t33ED521BE3A7E943FA8D764514952EDF1AF1C0FA* ___onValidateValue;
	int32_t ___U3CdispatchModeU3Ek__BackingField;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___U3ClabelElementU3Ek__BackingField;
	bool ___m_ShowMixedValue;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___m_MixedValueLabel;
	bool ___m_SkipValidation;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedContextWidthElement;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedInspectorElement;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingStartedU3Ek__BackingField;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingEndedU3Ek__BackingField;
};
struct BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B  : public BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C
{
	float ___m_LabelWidthRatio;
	float ___m_LabelExtraPadding;
	float ___m_LabelBaseMinWidth;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_VisualInput;
	Action_1_tAE453A6458E1DAD68CAF44DC8FF62EC18FEAA930* ___expressionEvaluated;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___viewDataRestored;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___m_Value;
	Func_2_t5FAD225BE5BF75673982C9FE55604AC36DBC1DB4* ___onValidateValue;
	int32_t ___U3CdispatchModeU3Ek__BackingField;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___U3ClabelElementU3Ek__BackingField;
	bool ___m_ShowMixedValue;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___m_MixedValueLabel;
	bool ___m_SkipValidation;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedContextWidthElement;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedInspectorElement;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingStartedU3Ek__BackingField;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingEndedU3Ek__BackingField;
};
struct BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F  : public BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C
{
	float ___m_LabelWidthRatio;
	float ___m_LabelExtraPadding;
	float ___m_LabelBaseMinWidth;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_VisualInput;
	Action_1_tAE453A6458E1DAD68CAF44DC8FF62EC18FEAA930* ___expressionEvaluated;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___viewDataRestored;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___m_Value;
	Func_2_t45386234ABB726FFBC7310E0192AE01A58728C52* ___onValidateValue;
	int32_t ___U3CdispatchModeU3Ek__BackingField;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___U3ClabelElementU3Ek__BackingField;
	bool ___m_ShowMixedValue;
	Label_tC160668F9119CE0F5567021FB208E64A5B1C5B70* ___m_MixedValueLabel;
	bool ___m_SkipValidation;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedContextWidthElement;
	VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___m_CachedInspectorElement;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingStartedU3Ek__BackingField;
	Action_tD00B0A84D7945E50C2DFFC28EFEE6ED44ED2AD07* ___U3CUnityEngine_UIElements_IEditableElement_editingEndedU3Ek__BackingField;
};
struct BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E : public BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C {};
struct RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014  : public BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507
{
};
struct RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7  : public BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B
{
};
struct RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7  : public BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F
{
};
struct RuntimeBaseFieldWrapper_1_t8D205E3CBF43B1E5DB0FA8F4A4914FE9FEF31653 : public BindableElement_t873EFF65032D21AB3B7BFBA21675D1693967435C {};
struct TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2  : public BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7
{
	TextInputBase_t1C4ADC0DE8BF430AA17D822D068E2204405C139A* ___m_TextInputBase;
	bool ___m_UpdateTextFromValue;
};
struct TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B  : public TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2
{
	BaseFieldMouseDragger_t6BB06D66A91C4C7DF3B50011F54727FB8CE92ED7* ___m_Dragger;
	bool ___m_ForceUpdateDisplay;
	bool ___m_SupportExpressions;
};
struct FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0  : public TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B
{
};
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_StaticFields
{
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___invariant_culture_info;
	RuntimeObject* ___shared_table_lock;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___default_current_culture;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___s_DefaultThreadCurrentUICulture;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___s_DefaultThreadCurrentCulture;
	Dictionary_2_t9FA6D82CAFC18769F7515BB51D1C56DAE09381C3* ___shared_by_number;
	Dictionary_2_tE1603CE612C16451D1E56FF4D4859D4FE4087C28* ___shared_by_name;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___s_UserPreferredCultureInfoInAppX;
	bool ___IsTaiwanSku;
};
struct NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472_StaticFields
{
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___invariantInfo;
};
struct String_t_StaticFields
{
	String_t* ___Empty;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17_StaticFields
{
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___s_categoryForLatin1;
};
struct Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___focusableProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___tabIndexProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___delegatesFocusProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___canGrabFocusProperty;
};
struct IntPtr_t_StaticFields
{
	intptr_t ___Zero;
};
struct Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_StaticFields
{
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___zeroVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___oneVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___upVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___downVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___leftVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___rightVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___positiveInfinityVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___negativeInfinityVector;
};
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_StaticFields
{
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___zeroVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___oneVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___upVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___downVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___leftVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___rightVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___forwardVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___backVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___positiveInfinityVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___negativeInfinityVector;
};
struct Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_StaticFields
{
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___zeroVector;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___oneVector;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___positiveInfinityVector;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___negativeInfinityVector;
};
struct Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields
{
	int32_t ___s_count;
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___s_zero;
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___s_one;
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___s_allOnes;
};
struct Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields
{
	int32_t ___s_count;
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___s_zero;
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___s_one;
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___s_allOnes;
};
struct Type_t_StaticFields
{
	Binder_t91BFCE95A7057FADF4D8A1A342AFE52872246235* ___s_defaultBinder;
	Il2CppChar ___Delimiter;
	TypeU5BU5D_t97234E1129B564EB38B8D85CAC2AD8B5B9522FFB* ___EmptyTypes;
	RuntimeObject* ___Missing;
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterAttribute;
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterName;
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterNameIgnoreCase;
};
struct Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED_StaticFields
{
	Action_3_tCDDEBF125C30A90B3A5061DE417B889F78E7DB83* ___onWidgetValueChangedAnalytic;
};
struct Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18_StaticFields
{
	Action_3_tAD728960C80D3C14B956508C335D759770FE2F6E* ___onWidgetValueChangedAnalytic;
};
struct Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD_StaticFields
{
	Action_3_t57DE42DCD9F152289CA8303B0B1AB7246E7FE864* ___onWidgetValueChangedAnalytic;
};
struct Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258_StaticFields
{
	Action_3_tE77469DC1E6595CCDCD9A1404CBA045A1C0AA560* ___onWidgetValueChangedAnalytic;
};
struct VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_StaticFields
{
	uint32_t ___s_NextId;
	List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* ___s_EmptyClassList;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___userDataPropertyKey;
	String_t* ___disabledUssClassName;
	int32_t ___s_FinalizerCount;
	ProfilerMarker_tA256E18DA86EDBC5528CE066FC91C96EE86501AD ___k_GenerateVisualContentMarker;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___childCountProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___contentRectProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___dataSourcePathProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___dataSourceProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___disablePlayModeTintProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___enabledInHierarchyProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___enabledSelfProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___layoutProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___languageDirectionProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___localBoundProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___nameProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___panelProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___pickingModeProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___styleSheetsProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___tooltipProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___usageHintsProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___userDataProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___viewDataKeyProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___visibleProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___visualTreeAssetSourceProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___worldBoundProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___worldTransformProperty;
	uint32_t ___s_NextParentVersion;
	List_1_t6115BBE78FE9310B180A2027321DF46F2A06AC95* ___s_EmptyList;
	Regex_tE773142C2BE45C5D362B0F815AFF831707A51772* ___s_InternalStyleSheetPath;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___tooltipPropertyKey;
	Dictionary_2_t4055F6540F36F21F9FEDAFB92D8E0089B38EBBC8* ___s_TypeData;
};
struct BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___valueProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___labelProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___showMixedValueProperty;
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
	String_t* ___noLabelVariantUssClassName;
	String_t* ___labelDraggerVariantUssClassName;
	String_t* ___mixedValueLabelUssClassName;
	String_t* ___alignedFieldUssClassName;
	String_t* ___inspectorFieldUssClassName;
	String_t* ___mixedValueString;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___serializedPropertyCopyName;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelWidthRatioProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelExtraPaddingProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelBaseMinWidthProperty;
};
struct BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___valueProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___labelProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___showMixedValueProperty;
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
	String_t* ___noLabelVariantUssClassName;
	String_t* ___labelDraggerVariantUssClassName;
	String_t* ___mixedValueLabelUssClassName;
	String_t* ___alignedFieldUssClassName;
	String_t* ___inspectorFieldUssClassName;
	String_t* ___mixedValueString;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___serializedPropertyCopyName;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelWidthRatioProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelExtraPaddingProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelBaseMinWidthProperty;
};
struct BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___valueProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___labelProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___showMixedValueProperty;
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
	String_t* ___noLabelVariantUssClassName;
	String_t* ___labelDraggerVariantUssClassName;
	String_t* ___mixedValueLabelUssClassName;
	String_t* ___alignedFieldUssClassName;
	String_t* ___inspectorFieldUssClassName;
	String_t* ___mixedValueString;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___serializedPropertyCopyName;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelWidthRatioProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelExtraPaddingProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelBaseMinWidthProperty;
};
struct BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___valueProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___labelProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___showMixedValueProperty;
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
	String_t* ___noLabelVariantUssClassName;
	String_t* ___labelDraggerVariantUssClassName;
	String_t* ___mixedValueLabelUssClassName;
	String_t* ___alignedFieldUssClassName;
	String_t* ___inspectorFieldUssClassName;
	String_t* ___mixedValueString;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___serializedPropertyCopyName;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelWidthRatioProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelExtraPaddingProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelBaseMinWidthProperty;
};
struct BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___valueProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___labelProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___showMixedValueProperty;
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
	String_t* ___noLabelVariantUssClassName;
	String_t* ___labelDraggerVariantUssClassName;
	String_t* ___mixedValueLabelUssClassName;
	String_t* ___alignedFieldUssClassName;
	String_t* ___inspectorFieldUssClassName;
	String_t* ___mixedValueString;
	PropertyName_tE4B4AAA58AF3BF2C0CD95509EB7B786F096901C2 ___serializedPropertyCopyName;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelWidthRatioProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelExtraPaddingProperty;
	CustomStyleProperty_1_t21332918528099194FD36C74FF0FA14696F39493 ___s_LabelBaseMinWidthProperty;
};
struct TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___autoCorrectionProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___hideMobileInputProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___hideSoftKeyboardProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___hidePlaceholderOnFocusProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___keyboardTypeProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___isReadOnlyProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___isPasswordFieldProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___textSelectionProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___textEditionProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___placeholderTextProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___cursorIndexProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___cursorPositionProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___selectIndexProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___selectAllOnFocusProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___selectAllOnMouseUpProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___maxLengthProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___doubleClickSelectsWordProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___tripleClickSelectsLineProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___emojiFallbackSupportProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___isDelayedProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___maskCharProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___verticalScrollerVisibilityProperty;
	CustomStyleProperty_1_tE4B20CAB5BCFEE711EB4A26F077DC700987C0C2D ___s_SelectionColorProperty;
	CustomStyleProperty_1_tE4B20CAB5BCFEE711EB4A26F077DC700987C0C2D ___s_CursorColorProperty;
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
	String_t* ___multilineContainerClassName;
	String_t* ___singleLineInputUssClassName;
	String_t* ___multilineInputUssClassName;
	String_t* ___placeholderUssClassName;
	String_t* ___multilineInputWithScrollViewUssClassName;
	String_t* ___textInputUssName;
};
struct TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B_StaticFields
{
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___formatStringProperty;
	BindingId_t8BBB6188CD126EACCA07816C78760E92DC16620E ___supportExpressionsProperty;
};
struct FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_StaticFields
{
	String_t* ___ussClassName;
	String_t* ___labelUssClassName;
	String_t* ___inputUssClassName;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif
struct FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F  : public RuntimeArray
{
	ALIGN_FIELD (8) FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* m_Items[1];

	inline FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248  : public RuntimeArray
{
	ALIGN_FIELD (8) String_t* m_Items[1];

	inline String_t* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline String_t** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, String_t* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline String_t* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline String_t** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, String_t* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
struct SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C  : public RuntimeArray
{
	ALIGN_FIELD (8) float m_Items[1];

	inline float GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline float* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, float value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline float GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline float* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, float value)
	{
		m_Items[index] = value;
	}
};


IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* VectorField_1_CreateRuntimeVectorField_mCF774A1DD5D431F0BF7A3CA72E1946E3CFF69653_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void BaseFieldHelper_ConfigureBaseField_TisVector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_mFE384414DF7A0BD2C355B0A6C6CC1C9A2E8FDA7A_gshared (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED* ___0_widget, BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* ___1_field, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass5_0__ctor_m82A36574B051BC139D9B177487EEAC11E5120491_gshared (U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR StyleEnum_1_tAFD4AA79F79D19DE23805D46DA67778A70EBBF26 StyleEnum_1_op_Implicit_mBB855459B336C069B9D8EC8EE64FAD9E840AB5A6_gshared (int32_t ___0_v, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_m1FB913858A7DD5A5D4AF808DEC7E4D8D286C4D29_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Func_1__ctor_m5FD0E1320A7A8964A7FCC0D0EA7F809E4E76E0EF_gshared (Func_1_t566D4A654882E282E54CDC3A001E05486F913903* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void RuntimeBaseFieldWrapper_1__ctor_m7630B2C09F5697B3DFE2B51072AAB475755F76C4_gshared (RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_container, String_t* ___1_label, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass6_0__ctor_m21CDD5EA8DA33DB4265FC30A11335B2E82CCF734_gshared (U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_gshared (TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B* __this, String_t* ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859_gshared (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71_gshared (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501_gshared (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_gshared (TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2* ___0_field, Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* ___1_onValueChanged, Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* ___2_getValue, Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* ___3_onDecrement, Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* ___4_onIncrement, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void EventCallback_1__ctor_mB1310964EAB7BF87F7ABF58378FA7A27145DB41A_gshared (EventCallback_1_t5B1D52718EA71C8F33D11D62351463B137A59482* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CallbackEventHandler_RegisterCallback_TisIl2CppSharedGenericObject_mADA60DA6265A096768601EDE87108F04A585DF5C_gshared (CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4* __this, EventCallback_1_t5B1D52718EA71C8F33D11D62351463B137A59482* ___0_callback, int32_t ___1_useTrickleDown, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_gshared (BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7* __this, String_t* ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 Field_1_GetValue_m35BC0454EB241EF390E1AC4F4E9FFDF5286BDCA1_gshared (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_m743A6409D098F26A091B4EAF78749892CBA3F021_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Field_1__ctor_m7C303BA68691F2521E9EC689B23AE797B4DFBB05_gshared (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* VectorField_1_CreateRuntimeVectorField_m086D519FFD86271A500C37EECA4F5736D8A18CE4_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void BaseFieldHelper_ConfigureBaseField_TisVector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_mEABD641224D190E163CBC2A813D1E24148F1DAA3_gshared (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18* ___0_widget, BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* ___1_field, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass5_0__ctor_mB45641A30C6F56A00296C6AA5FC2E88108F6D8E9_gshared (U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_mBA6A2A437F8CABA005347D6D7A586B0CEC510D72_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void RuntimeBaseFieldWrapper_1__ctor_m45AE15129A72C4E53C8219F3FED6CB2B8823E369_gshared (RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_container, String_t* ___1_label, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass6_0__ctor_mB87D4FA5A89373F41711521900B5FAB81717C133_gshared (U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Field_1_GetValue_m64B831DFD988D14A1DA8AD2548F2B1D1867218CC_gshared (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_mD23EF8338D2FE25735FF01F04C1A5EAE4797BD75_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Field_1__ctor_m5E85360C971446C73E1A8E5ED7DA17D7EDC90E1B_gshared (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* VectorField_1_CreateRuntimeVectorField_m8CF1C678A918D8CC2829C6A4DCA4A4527975BC4E_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void BaseFieldHelper_ConfigureBaseField_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mC7D4E12978C810E289F2F469779BF6FEC8D368EE_gshared (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD* ___0_widget, BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* ___1_field, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass5_0__ctor_mB73C181FA5C0498C3141BAA1FDC95F3A13FA6B46_gshared (U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_mE9B22CD2331602818289DC43C983A1C0CF08A160_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void RuntimeBaseFieldWrapper_1__ctor_mB2BF8A0896212028CB08FE015B29DD4655D58113_gshared (RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_container, String_t* ___1_label, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass6_0__ctor_m3809B224386D2FB43C8E6BFDE9FDE0AD6E91B5F3_gshared (U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 Field_1_GetValue_m36C6FCEF8084FFAC4351BB53A298145B627A4CA6_gshared (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_mC184D222EEF986D89732893626E7FD35F5611DB3_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Field_1__ctor_m987712BF4E8BBF11473DA83B4CF70877C002430C_gshared (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_gshared_inline (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, uint16_t ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, void* ___0_dataPointer, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint16_t Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, int32_t ___0_index, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_other, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_gshared_inline (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_gshared_inline (uint16_t ___0_left, uint16_t ___1_right, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_GetHashCode_m3C7CFE908C6BB2DC94F94F7615F2D1AF0E2777D9_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_m2444D8FDCF0568D259DAE989EB7BCC77D37B2D6D_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_op_Equality_mB42F3DAE52C3BC7579B302E623196C45A5DEAC6B_gshared_inline (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_left, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___1_right, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_InitializeCount_m43BBDDA05FDAB290038584331DB79CB33C523B83_gshared (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint16_t Vector_1_GetOneValue_m7E814AFD17E4D390C12EF731DA01203D262D9953_gshared_inline (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint16_t Vector_1_GetAllBitsSetValue_m854DE079EA89F97089D3EF29D7C31F081F420580_gshared_inline (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_gshared_inline (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, uint64_t ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, void* ___0_dataPointer, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint64_t Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, int32_t ___0_index, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_other, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_gshared_inline (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_gshared_inline (uint64_t ___0_left, uint64_t ___1_right, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_GetHashCode_mEC951E56E2DC500CF877DFAD5542E0920B73B00A_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_m3EBF88D3E195BD2C4B0D1CCBD9F71E32233CA4F4_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_op_Equality_mD4D4AE7733CACE50CA2FCFFFB0A16818EEC01293_gshared_inline (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_left, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___1_right, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_InitializeCount_mE29E088973A17B81B830C30831075135FC8E263A_gshared (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint64_t Vector_1_GetOneValue_mE2DE5D8CFC8D7A4990743C160CD1C4ED71CDA288_gshared_inline (const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint64_t Vector_1_GetAllBitsSetValue_m99E582A6A7DA5089B26FE42E5F8FDE26A6005ED0_gshared_inline (const RuntimeMethod* method) ;

IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool DebugUIExtensions_IsAnyRuntimeContext_mF1F32BD66156D14BC1CD24234D8EEAAE324EFB5D (int32_t ___0_context, const RuntimeMethod* method) ;
inline BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* VectorField_1_CreateRuntimeVectorField_mCF774A1DD5D431F0BF7A3CA72E1946E3CFF69653 (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method)
{
	return ((  BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* (*) (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87*, const RuntimeMethod*))VectorField_1_CreateRuntimeVectorField_mCF774A1DD5D431F0BF7A3CA72E1946E3CFF69653_gshared)(__this, method);
}
inline void BaseFieldHelper_ConfigureBaseField_TisVector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_mFE384414DF7A0BD2C355B0A6C6CC1C9A2E8FDA7A (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED* ___0_widget, BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* ___1_field, const RuntimeMethod* method)
{
	((  void (*) (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*, BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507*, const RuntimeMethod*))BaseFieldHelper_ConfigureBaseField_TisVector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_mFE384414DF7A0BD2C355B0A6C6CC1C9A2E8FDA7A_gshared)(___0_widget, ___1_field, method);
}
inline void U3CU3Ec__DisplayClass5_0__ctor_m82A36574B051BC139D9B177487EEAC11E5120491 (U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* __this, const RuntimeMethod* method)
{
	((  void (*) (U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402*, const RuntimeMethod*))U3CU3Ec__DisplayClass5_0__ctor_m82A36574B051BC139D9B177487EEAC11E5120491_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VisualElement__ctor_m4C59A7BA0CE74223A61F07C39A60071DD0207E2D (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B (String_t* ___0_str0, String_t* ___1_str1, String_t* ___2_str2, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VisualElement_AddToClassList_mAF0DD8D8CFD5130229A0471DD260E01ED82117F1 (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* __this, String_t* ___0_className, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* VisualElement_get_style_mDCFF8D835BE0AFE412905E108F48B32A83734224 (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* __this, const RuntimeMethod* method) ;
inline StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD (int32_t ___0_v, const RuntimeMethod* method)
{
	return ((  StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE (*) (int32_t, const RuntimeMethod*))StyleEnum_1_op_Implicit_mBB855459B336C069B9D8EC8EE64FAD9E840AB5A6_gshared)(___0_v, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Focusable_set_delegatesFocus_mC691C4199C88BEF0C55A7F7FD2C6ADDD00402D6F (Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0* __this, bool ___0_value, const RuntimeMethod* method) ;
inline FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_m1FB913858A7DD5A5D4AF808DEC7E4D8D286C4D29 (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method)
{
	return ((  FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* (*) (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87*, const RuntimeMethod*))VectorField_1_CreateComponentFields_m1FB913858A7DD5A5D4AF808DEC7E4D8D286C4D29_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VisualElement_Add_mE2571CCB23C09103F8732EEC73833683F7236A7F (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_child, const RuntimeMethod* method) ;
inline void Func_1__ctor_m4D45A6927B7EBCE17611779C8DD2E5257CB11922 (Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780*, RuntimeObject*, intptr_t, const RuntimeMethod*))Func_1__ctor_m5FD0E1320A7A8964A7FCC0D0EA7F809E4E76E0EF_gshared)(__this, ___0_object, ___1_method, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void DebugUIExtensions_ScheduleTracked_mC19444ADBEE6613D2D7C121BCD5B6F2E8233EDEE (Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF* ___0_widget, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___1_element, Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780* ___2_scheduledItemCreator, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline (Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF* __this, const RuntimeMethod* method) ;
inline void RuntimeBaseFieldWrapper_1__ctor_m7630B2C09F5697B3DFE2B51072AAB475755F76C4 (RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_container, String_t* ___1_label, const RuntimeMethod* method)
{
	((  void (*) (RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014*, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*, String_t*, const RuntimeMethod*))RuntimeBaseFieldWrapper_1__ctor_m7630B2C09F5697B3DFE2B51072AAB475755F76C4_gshared)(__this, ___0_container, ___1_label, method);
}
inline void U3CU3Ec__DisplayClass6_0__ctor_m21CDD5EA8DA33DB4265FC30A11335B2E82CCF734 (U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* __this, const RuntimeMethod* method)
{
	((  void (*) (U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE*, const RuntimeMethod*))U3CU3Ec__DisplayClass6_0__ctor_m21CDD5EA8DA33DB4265FC30A11335B2E82CCF734_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FloatField__ctor_m46165DF8164AD0A15CBD7FACE88D8902BE113696 (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Format_mA8DBB4C2516B9723C5A41E6CB1E2FAF4BBE96DD8 (String_t* ___0_format, RuntimeObject* ___1_arg0, const RuntimeMethod* method) ;
inline void TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552 (TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B* __this, String_t* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*, String_t*, const RuntimeMethod*))TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_gshared)(__this, ___0_value, method);
}
inline void Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859 (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A*, RuntimeObject*, intptr_t, const RuntimeMethod*))Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859_gshared)(__this, ___0_object, ___1_method, method);
}
inline void Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71 (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7*, RuntimeObject*, intptr_t, const RuntimeMethod*))Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71_gshared)(__this, ___0_object, ___1_method, method);
}
inline void Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501 (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*, RuntimeObject*, intptr_t, const RuntimeMethod*))Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501_gshared)(__this, ___0_object, ___1_method, method);
}
inline void DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A (TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2* ___0_field, Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* ___1_onValueChanged, Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* ___2_getValue, Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* ___3_onDecrement, Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* ___4_onIncrement, const RuntimeMethod* method)
{
	((  void (*) (TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2*, Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A*, Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7*, Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*, Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*, const RuntimeMethod*))DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_gshared)(___0_field, ___1_onValueChanged, ___2_getValue, ___3_onDecrement, ___4_onIncrement, method);
}
inline void EventCallback_1__ctor_m419A2A0D4A537D68FC86F3EBA2998FB37CA6F42E (EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07*, RuntimeObject*, intptr_t, const RuntimeMethod*))EventCallback_1__ctor_mB1310964EAB7BF87F7ABF58378FA7A27145DB41A_gshared)(__this, ___0_object, ___1_method, method);
}
inline void CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED (CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4* __this, EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07* ___0_callback, int32_t ___1_useTrickleDown, const RuntimeMethod* method)
{
	((  void (*) (CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*, EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07*, int32_t, const RuntimeMethod*))CallbackEventHandler_RegisterCallback_TisIl2CppSharedGenericObject_mADA60DA6265A096768601EDE87108F04A585DF5C_gshared)(__this, ___0_callback, ___1_useTrickleDown, method);
}
inline void BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F (BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7* __this, String_t* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*, String_t*, const RuntimeMethod*))BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_gshared)(__this, ___0_value, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VisualElement_set_tooltip_mD392FEBD1C4F97E4FEE2FEE3E92937D355B7FA61 (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* __this, String_t* ___0_value, const RuntimeMethod* method) ;
inline Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 Field_1_GetValue_m35BC0454EB241EF390E1AC4F4E9FFDF5286BDCA1 (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED* __this, const RuntimeMethod* method)
{
	return ((  Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 (*) (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*, const RuntimeMethod*))Field_1_GetValue_m35BC0454EB241EF390E1AC4F4E9FFDF5286BDCA1_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR float DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9 (float ___0_value, SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* ___1_referenceValues, const RuntimeMethod* method) ;
inline void VectorField_1_UpdateVectorComponent_m743A6409D098F26A091B4EAF78749892CBA3F021 (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method)
{
	((  void (*) (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87*, int32_t, float, const RuntimeMethod*))VectorField_1_UpdateVectorComponent_m743A6409D098F26A091B4EAF78749892CBA3F021_gshared)(__this, ___0_componentIndex, ___1_newValue, method);
}
inline void Field_1__ctor_m7C303BA68691F2521E9EC689B23AE797B4DFBB05 (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED* __this, const RuntimeMethod* method)
{
	((  void (*) (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*, const RuntimeMethod*))Field_1__ctor_m7C303BA68691F2521E9EC689B23AE797B4DFBB05_gshared)(__this, method);
}
inline BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* VectorField_1_CreateRuntimeVectorField_m086D519FFD86271A500C37EECA4F5736D8A18CE4 (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method)
{
	return ((  BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* (*) (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D*, const RuntimeMethod*))VectorField_1_CreateRuntimeVectorField_m086D519FFD86271A500C37EECA4F5736D8A18CE4_gshared)(__this, method);
}
inline void BaseFieldHelper_ConfigureBaseField_TisVector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_mEABD641224D190E163CBC2A813D1E24148F1DAA3 (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18* ___0_widget, BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* ___1_field, const RuntimeMethod* method)
{
	((  void (*) (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*, BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B*, const RuntimeMethod*))BaseFieldHelper_ConfigureBaseField_TisVector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_mEABD641224D190E163CBC2A813D1E24148F1DAA3_gshared)(___0_widget, ___1_field, method);
}
inline void U3CU3Ec__DisplayClass5_0__ctor_mB45641A30C6F56A00296C6AA5FC2E88108F6D8E9 (U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* __this, const RuntimeMethod* method)
{
	((  void (*) (U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45*, const RuntimeMethod*))U3CU3Ec__DisplayClass5_0__ctor_mB45641A30C6F56A00296C6AA5FC2E88108F6D8E9_gshared)(__this, method);
}
inline FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_mBA6A2A437F8CABA005347D6D7A586B0CEC510D72 (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method)
{
	return ((  FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* (*) (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D*, const RuntimeMethod*))VectorField_1_CreateComponentFields_mBA6A2A437F8CABA005347D6D7A586B0CEC510D72_gshared)(__this, method);
}
inline void RuntimeBaseFieldWrapper_1__ctor_m45AE15129A72C4E53C8219F3FED6CB2B8823E369 (RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_container, String_t* ___1_label, const RuntimeMethod* method)
{
	((  void (*) (RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7*, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*, String_t*, const RuntimeMethod*))RuntimeBaseFieldWrapper_1__ctor_m45AE15129A72C4E53C8219F3FED6CB2B8823E369_gshared)(__this, ___0_container, ___1_label, method);
}
inline void U3CU3Ec__DisplayClass6_0__ctor_mB87D4FA5A89373F41711521900B5FAB81717C133 (U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* __this, const RuntimeMethod* method)
{
	((  void (*) (U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00*, const RuntimeMethod*))U3CU3Ec__DisplayClass6_0__ctor_mB87D4FA5A89373F41711521900B5FAB81717C133_gshared)(__this, method);
}
inline Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 Field_1_GetValue_m64B831DFD988D14A1DA8AD2548F2B1D1867218CC (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18* __this, const RuntimeMethod* method)
{
	return ((  Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 (*) (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*, const RuntimeMethod*))Field_1_GetValue_m64B831DFD988D14A1DA8AD2548F2B1D1867218CC_gshared)(__this, method);
}
inline void VectorField_1_UpdateVectorComponent_mD23EF8338D2FE25735FF01F04C1A5EAE4797BD75 (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method)
{
	((  void (*) (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D*, int32_t, float, const RuntimeMethod*))VectorField_1_UpdateVectorComponent_mD23EF8338D2FE25735FF01F04C1A5EAE4797BD75_gshared)(__this, ___0_componentIndex, ___1_newValue, method);
}
inline void Field_1__ctor_m5E85360C971446C73E1A8E5ED7DA17D7EDC90E1B (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18* __this, const RuntimeMethod* method)
{
	((  void (*) (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*, const RuntimeMethod*))Field_1__ctor_m5E85360C971446C73E1A8E5ED7DA17D7EDC90E1B_gshared)(__this, method);
}
inline BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* VectorField_1_CreateRuntimeVectorField_m8CF1C678A918D8CC2829C6A4DCA4A4527975BC4E (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method)
{
	return ((  BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* (*) (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3*, const RuntimeMethod*))VectorField_1_CreateRuntimeVectorField_m8CF1C678A918D8CC2829C6A4DCA4A4527975BC4E_gshared)(__this, method);
}
inline void BaseFieldHelper_ConfigureBaseField_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mC7D4E12978C810E289F2F469779BF6FEC8D368EE (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD* ___0_widget, BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* ___1_field, const RuntimeMethod* method)
{
	((  void (*) (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*, BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F*, const RuntimeMethod*))BaseFieldHelper_ConfigureBaseField_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mC7D4E12978C810E289F2F469779BF6FEC8D368EE_gshared)(___0_widget, ___1_field, method);
}
inline void U3CU3Ec__DisplayClass5_0__ctor_mB73C181FA5C0498C3141BAA1FDC95F3A13FA6B46 (U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* __this, const RuntimeMethod* method)
{
	((  void (*) (U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807*, const RuntimeMethod*))U3CU3Ec__DisplayClass5_0__ctor_mB73C181FA5C0498C3141BAA1FDC95F3A13FA6B46_gshared)(__this, method);
}
inline FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_mE9B22CD2331602818289DC43C983A1C0CF08A160 (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method)
{
	return ((  FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* (*) (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3*, const RuntimeMethod*))VectorField_1_CreateComponentFields_mE9B22CD2331602818289DC43C983A1C0CF08A160_gshared)(__this, method);
}
inline void RuntimeBaseFieldWrapper_1__ctor_mB2BF8A0896212028CB08FE015B29DD4655D58113 (RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7* __this, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* ___0_container, String_t* ___1_label, const RuntimeMethod* method)
{
	((  void (*) (RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7*, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*, String_t*, const RuntimeMethod*))RuntimeBaseFieldWrapper_1__ctor_mB2BF8A0896212028CB08FE015B29DD4655D58113_gshared)(__this, ___0_container, ___1_label, method);
}
inline void U3CU3Ec__DisplayClass6_0__ctor_m3809B224386D2FB43C8E6BFDE9FDE0AD6E91B5F3 (U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* __this, const RuntimeMethod* method)
{
	((  void (*) (U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5*, const RuntimeMethod*))U3CU3Ec__DisplayClass6_0__ctor_m3809B224386D2FB43C8E6BFDE9FDE0AD6E91B5F3_gshared)(__this, method);
}
inline Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 Field_1_GetValue_m36C6FCEF8084FFAC4351BB53A298145B627A4CA6 (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD* __this, const RuntimeMethod* method)
{
	return ((  Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 (*) (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*, const RuntimeMethod*))Field_1_GetValue_m36C6FCEF8084FFAC4351BB53A298145B627A4CA6_gshared)(__this, method);
}
inline void VectorField_1_UpdateVectorComponent_mC184D222EEF986D89732893626E7FD35F5611DB3 (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method)
{
	((  void (*) (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3*, int32_t, float, const RuntimeMethod*))VectorField_1_UpdateVectorComponent_mC184D222EEF986D89732893626E7FD35F5611DB3_gshared)(__this, ___0_componentIndex, ___1_newValue, method);
}
inline void Field_1__ctor_m987712BF4E8BBF11473DA83B4CF70877C002430C (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD* __this, const RuntimeMethod* method)
{
	((  void (*) (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*, const RuntimeMethod*))Field_1__ctor_m987712BF4E8BBF11473DA83B4CF70877C002430C_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA (const RuntimeMethod* method) ;
inline int32_t Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline (const RuntimeMethod* method)
{
	return ((  int32_t (*) (const RuntimeMethod*))Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_gshared_inline)(method);
}
inline void Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, uint16_t ___0_value, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, uint16_t, const RuntimeMethod*))Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427_gshared)(__this, ___0_value, method);
}
inline void Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, void*, int32_t, const RuntimeMethod*))Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359_gshared)(__this, ___0_dataPointer, ___1_offset, method);
}
inline void Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, void* ___0_dataPointer, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, void*, const RuntimeMethod*))Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3_gshared)(__this, ___0_dataPointer, method);
}
inline void Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*, const RuntimeMethod*))Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A_gshared)(__this, ___0_existingRegister, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* SR_Format_m9E8DC9AEFDC34AC67473EFAEAB78C5066C1A0D09 (String_t* ___0_resourceFormat, RuntimeObject* ___1_p1, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IndexOutOfRangeException__ctor_mFD06819F05B815BE2D6E826D4E04F4C449D0A425 (IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82* __this, String_t* ___0_message, const RuntimeMethod* method) ;
inline uint16_t Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	return ((  uint16_t (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, int32_t, const RuntimeMethod*))Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27_gshared)(__this, ___0_index, method);
}
inline bool Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_other, const RuntimeMethod* method)
{
	return ((  bool (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489, const RuntimeMethod*))Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8_gshared)(__this, ___0_other, method);
}
inline bool Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_inline (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method)
{
	return ((  bool (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, RuntimeObject*, const RuntimeMethod*))Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_gshared_inline)(__this, ___0_obj, method);
}
inline bool Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_inline (uint16_t ___0_left, uint16_t ___1_right, const RuntimeMethod* method)
{
	return ((  bool (*) (uint16_t, uint16_t, const RuntimeMethod*))Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_gshared_inline)(___0_left, ___1_right, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200 (uint16_t* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7 (int32_t ___0_h1, int32_t ___1_h2, const RuntimeMethod* method) ;
inline int32_t Vector_1_GetHashCode_m3C7CFE908C6BB2DC94F94F7615F2D1AF0E2777D9 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, const RuntimeMethod*))Vector_1_GetHashCode_m3C7CFE908C6BB2DC94F94F7615F2D1AF0E2777D9_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* CultureInfo_get_CurrentCulture_m8A4580F49DDD7E9DB34C699965423DB8E3BBA9A5 (const RuntimeMethod* method) ;
inline String_t* Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7 (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method)
{
	return ((  String_t* (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, String_t*, RuntimeObject*, const RuntimeMethod*))Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7_gshared)(__this, ___0_format, ___1_formatProvider, method);
}
inline String_t* Vector_1_ToString_m2444D8FDCF0568D259DAE989EB7BCC77D37B2D6D (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, const RuntimeMethod* method)
{
	return ((  String_t* (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*, const RuntimeMethod*))Vector_1_ToString_m2444D8FDCF0568D259DAE989EB7BCC77D37B2D6D_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void StringBuilder__ctor_m1D99713357DE05DAFA296633639DB55F8C30587D (StringBuilder_t* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* NumberFormatInfo_GetInstance_m705987E5E7D3E5EC5C5DD2D088FBC9BCBA0FC31F (RuntimeObject* ___0_formatProvider, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* NumberFormatInfo_get_NumberGroupSeparator_m0556B092AA471513B1EDC31C047712226D39BEB6_inline (NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR StringBuilder_t* StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1 (StringBuilder_t* __this, Il2CppChar ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* UInt16_ToString_mBD648884B6569D3E7D779669EEFCB1ED5EE4A521 (uint16_t* __this, String_t* ___0_format, RuntimeObject* ___1_provider, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR StringBuilder_t* StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D (StringBuilder_t* __this, String_t* ___0_value, const RuntimeMethod* method) ;
inline bool Vector_1_op_Equality_mB42F3DAE52C3BC7579B302E623196C45A5DEAC6B_inline (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_left, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___1_right, const RuntimeMethod* method)
{
	return ((  bool (*) (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489, const RuntimeMethod*))Vector_1_op_Equality_mB42F3DAE52C3BC7579B302E623196C45A5DEAC6B_gshared_inline)(___0_left, ___1_right, method);
}
inline void Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606 (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*, const RuntimeMethod*))Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_gshared)(__this, ___0_existingRegister, method);
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint16_t ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline (const RuntimeMethod* method) ;
inline int32_t Vector_1_InitializeCount_m43BBDDA05FDAB290038584331DB79CB33C523B83 (const RuntimeMethod* method)
{
	return ((  int32_t (*) (const RuntimeMethod*))Vector_1_InitializeCount_m43BBDDA05FDAB290038584331DB79CB33C523B83_gshared)(method);
}
inline uint16_t Vector_1_GetOneValue_m7E814AFD17E4D390C12EF731DA01203D262D9953_inline (const RuntimeMethod* method)
{
	return ((  uint16_t (*) (const RuntimeMethod*))Vector_1_GetOneValue_m7E814AFD17E4D390C12EF731DA01203D262D9953_gshared_inline)(method);
}
inline uint16_t Vector_1_GetAllBitsSetValue_m854DE079EA89F97089D3EF29D7C31F081F420580_inline (const RuntimeMethod* method)
{
	return ((  uint16_t (*) (const RuntimeMethod*))Vector_1_GetAllBitsSetValue_m854DE079EA89F97089D3EF29D7C31F081F420580_gshared_inline)(method);
}
inline int32_t Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline (const RuntimeMethod* method)
{
	return ((  int32_t (*) (const RuntimeMethod*))Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_gshared_inline)(method);
}
inline void Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6 (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, uint64_t ___0_value, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, uint64_t, const RuntimeMethod*))Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6_gshared)(__this, ___0_value, method);
}
inline void Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, void*, int32_t, const RuntimeMethod*))Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B_gshared)(__this, ___0_dataPointer, ___1_offset, method);
}
inline void Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, void* ___0_dataPointer, const RuntimeMethod* method)
{
	((  void (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, void*, const RuntimeMethod*))Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA_gshared)(__this, ___0_dataPointer, method);
}
inline uint64_t Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99 (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	return ((  uint64_t (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, int32_t, const RuntimeMethod*))Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99_gshared)(__this, ___0_index, method);
}
inline bool Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27 (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_other, const RuntimeMethod* method)
{
	return ((  bool (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A, const RuntimeMethod*))Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27_gshared)(__this, ___0_other, method);
}
inline bool Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_inline (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method)
{
	return ((  bool (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, RuntimeObject*, const RuntimeMethod*))Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_gshared_inline)(__this, ___0_obj, method);
}
inline bool Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_inline (uint64_t ___0_left, uint64_t ___1_right, const RuntimeMethod* method)
{
	return ((  bool (*) (uint64_t, uint64_t, const RuntimeMethod*))Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_gshared_inline)(___0_left, ___1_right, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t UInt64_GetHashCode_m65D9FD0102B6B01BF38D986F060F0BDBC29B4F92 (uint64_t* __this, const RuntimeMethod* method) ;
inline int32_t Vector_1_GetHashCode_mEC951E56E2DC500CF877DFAD5542E0920B73B00A (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, const RuntimeMethod*))Vector_1_GetHashCode_mEC951E56E2DC500CF877DFAD5542E0920B73B00A_gshared)(__this, method);
}
inline String_t* Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method)
{
	return ((  String_t* (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, String_t*, RuntimeObject*, const RuntimeMethod*))Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C_gshared)(__this, ___0_format, ___1_formatProvider, method);
}
inline String_t* Vector_1_ToString_m3EBF88D3E195BD2C4B0D1CCBD9F71E32233CA4F4 (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, const RuntimeMethod* method)
{
	return ((  String_t* (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*, const RuntimeMethod*))Vector_1_ToString_m3EBF88D3E195BD2C4B0D1CCBD9F71E32233CA4F4_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* UInt64_ToString_m779041C8FDD58BF8617838B00CD041788DB2F1A3 (uint64_t* __this, String_t* ___0_format, RuntimeObject* ___1_provider, const RuntimeMethod* method) ;
inline bool Vector_1_op_Equality_mD4D4AE7733CACE50CA2FCFFFB0A16818EEC01293_inline (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_left, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___1_right, const RuntimeMethod* method)
{
	return ((  bool (*) (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A, const RuntimeMethod*))Vector_1_op_Equality_mD4D4AE7733CACE50CA2FCFFFB0A16818EEC01293_gshared_inline)(___0_left, ___1_right, method);
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint64_t ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline (const RuntimeMethod* method) ;
inline int32_t Vector_1_InitializeCount_mE29E088973A17B81B830C30831075135FC8E263A (const RuntimeMethod* method)
{
	return ((  int32_t (*) (const RuntimeMethod*))Vector_1_InitializeCount_mE29E088973A17B81B830C30831075135FC8E263A_gshared)(method);
}
inline uint64_t Vector_1_GetOneValue_mE2DE5D8CFC8D7A4990743C160CD1C4ED71CDA288_inline (const RuntimeMethod* method)
{
	return ((  uint64_t (*) (const RuntimeMethod*))Vector_1_GetOneValue_mE2DE5D8CFC8D7A4990743C160CD1C4ED71CDA288_gshared_inline)(method);
}
inline uint64_t Vector_1_GetAllBitsSetValue_m99E582A6A7DA5089B26FE42E5F8FDE26A6005ED0_inline (const RuntimeMethod* method)
{
	return ((  uint64_t (*) (const RuntimeMethod*))Vector_1_GetAllBitsSetValue_m99E582A6A7DA5089B26FE42E5F8FDE26A6005ED0_gshared_inline)(method);
}
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 22740
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* VectorField_1_Create_m28AE3E64AE4AE5128BCACE538221EBAAC869F0B9_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* V_0 = NULL;
	{
		int32_t L_0 = ((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this)->___m_Context;
		bool L_1;
		L_1 = DebugUIExtensions_IsAnyRuntimeContext_mF1F32BD66156D14BC1CD24234D8EEAAE324EFB5D(L_0, NULL);
		if (!L_1)
		{
			goto IL_0016;
		}
	}
	{
		BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* L_2;
		L_2 = VectorField_1_CreateRuntimeVectorField_mCF774A1DD5D431F0BF7A3CA72E1946E3CFF69653(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		V_0 = L_2;
		goto IL_001d;
	}

IL_0016:
	{
		BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* L_3;
		L_3 = VirtualFuncInvoker0< BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* >::Invoke(27, __this);
		V_0 = L_3;
	}

IL_001d:
	{
		BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* L_4 = V_0;
		BaseFieldHelper_ConfigureBaseField_TisVector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_mFE384414DF7A0BD2C355B0A6C6CC1C9A2E8FDA7A((Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*)__this, L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 4));
		BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* L_5 = V_0;
		return (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_5;
	}
}
// Method Definition Index: 22741
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507* VectorField_1_CreateRuntimeVectorField_mCF774A1DD5D431F0BF7A3CA72E1946E3CFF69653_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral34761125773C5996C26093DBBA81463705909069);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* V_0 = NULL;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	int32_t V_2 = 0;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_3 = NULL;
	{
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_0 = (U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6));
		U3CU3Ec__DisplayClass5_0__ctor_m82A36574B051BC139D9B177487EEAC11E5120491(L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		V_0 = L_0;
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_1 = V_0;
		NullCheck(L_1);
		L_1->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_1->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_2 = V_0;
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_3 = (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)il2cpp_codegen_object_new(VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		VisualElement__ctor_m4C59A7BA0CE74223A61F07C39A60071DD0207E2D(L_3, NULL);
		NullCheck(L_2);
		L_2->___fieldsContainer = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&L_2->___fieldsContainer), (void*)L_3);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_4 = V_0;
		NullCheck(L_4);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_5 = L_4->___fieldsContainer;
		String_t* L_6;
		L_6 = VirtualFuncInvoker0< String_t* >::Invoke(24, __this);
		String_t* L_7;
		L_7 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146, L_6, _stringLiteral34761125773C5996C26093DBBA81463705909069, NULL);
		NullCheck(L_5);
		VisualElement_AddToClassList_mAF0DD8D8CFD5130229A0471DD260E01ED82117F1(L_5, L_7, NULL);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_8 = V_0;
		NullCheck(L_8);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_9 = L_8->___fieldsContainer;
		NullCheck(L_9);
		RuntimeObject* L_10;
		L_10 = VisualElement_get_style_mDCFF8D835BE0AFE412905E108F48B32A83734224(L_9, NULL);
		StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE L_11;
		L_11 = StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD((int32_t)0, StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		NullCheck(L_10);
		InterfaceActionInvoker1< StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE >::Invoke(57, IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var, L_10, L_11);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_12 = V_0;
		NullCheck(L_12);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_13 = L_12->___fieldsContainer;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13);
		Focusable_set_delegatesFocus_mC691C4199C88BEF0C55A7F7FD2C6ADDD00402D6F((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13, (bool)1, NULL);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_14 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_15;
		L_15 = VectorField_1_CreateComponentFields_m1FB913858A7DD5A5D4AF808DEC7E4D8D286C4D29(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 9));
		NullCheck(L_14);
		L_14->___componentFields = L_15;
		Il2CppCodeGenWriteBarrier((void**)(&L_14->___componentFields), (void*)L_15);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_16 = V_0;
		NullCheck(L_16);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_17 = L_16->___componentFields;
		V_1 = L_17;
		V_2 = 0;
		goto IL_0085;
	}

IL_0071:
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_18 = V_1;
		int32_t L_19 = V_2;
		NullCheck(L_18);
		int32_t L_20 = L_19;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_21 = (L_18)->GetAt(static_cast<il2cpp_array_size_t>(L_20));
		V_3 = L_21;
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_22 = V_0;
		NullCheck(L_22);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_23 = L_22->___fieldsContainer;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_24 = V_3;
		NullCheck(L_23);
		VisualElement_Add_mE2571CCB23C09103F8732EEC73833683F7236A7F(L_23, (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_24, NULL);
		int32_t L_25 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_25, 1));
	}

IL_0085:
	{
		int32_t L_26 = V_2;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_27 = V_1;
		NullCheck(L_27);
		int32_t L_28 = (il2cpp_codegen_conv<int32_t,int64_t,int64_t,false,false>((((RuntimeArray*)L_27)->max_length),NULL));
		if ((((int32_t)L_26) < ((int32_t)L_28)))
		{
			goto IL_0071;
		}
	}
	{
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_29 = V_0;
		NullCheck(L_29);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_30 = L_29->___fieldsContainer;
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_31 = V_0;
		Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780* L_32 = (Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780*)il2cpp_codegen_object_new(Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		Func_1__ctor_m4D45A6927B7EBCE17611779C8DD2E5257CB11922(L_32, (RuntimeObject*)L_31, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 10)), NULL);
		DebugUIExtensions_ScheduleTracked_mC19444ADBEE6613D2D7C121BCD5B6F2E8233EDEE((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, L_30, L_32, NULL);
		U3CU3Ec__DisplayClass5_0_t3EB000EFC161B78F28BC9C9A4DB253F50764F402* L_33 = V_0;
		NullCheck(L_33);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_34 = L_33->___fieldsContainer;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_35;
		L_35 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014* L_36 = (RuntimeBaseFieldWrapper_1_t8E4EF008558C31865B7227760FFE49BE529A1014*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 11));
		RuntimeBaseFieldWrapper_1__ctor_m7630B2C09F5697B3DFE2B51072AAB475755F76C4(L_36, L_34, L_35, il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		return (BaseField_1_t24288AF0F89D70409E802DB92E87D9CA0A822507*)L_36;
	}
}
// Method Definition Index: 22742
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_m1FB913858A7DD5A5D4AF808DEC7E4D8D286C4D29_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* V_2 = NULL;
	int32_t V_3 = 0;
	U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* V_4 = NULL;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_5 = NULL;
	{
		int32_t L_0;
		L_0 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		V_0 = L_0;
		int32_t L_1 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_2 = (FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)SZArrayNew(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var, (uint32_t)L_1);
		V_1 = L_2;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_3;
		L_3 = VirtualFuncInvoker0< StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* >::Invoke(23, __this);
		V_2 = L_3;
		V_3 = 0;
		goto IL_00da;
	}

IL_001c:
	{
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_4 = (U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 15));
		U3CU3Ec__DisplayClass6_0__ctor_m21CDD5EA8DA33DB4265FC30A11335B2E82CCF734(L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		V_4 = L_4;
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_5 = V_4;
		NullCheck(L_5);
		L_5->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_5->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_6 = V_4;
		int32_t L_7 = V_3;
		NullCheck(L_6);
		L_6->___componentIndex = L_7;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_8 = (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)il2cpp_codegen_object_new(FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		FloatField__ctor_m46165DF8164AD0A15CBD7FACE88D8902BE113696(L_8, NULL);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_9 = L_8;
		int32_t L_10 = __this->___decimals;
		int32_t L_11 = L_10;
		RuntimeObject* L_12 = Box(il2cpp_defaults.int32_class, &L_11);
		String_t* L_13;
		L_13 = String_Format_mA8DBB4C2516B9723C5A41E6CB1E2FAF4BBE96DD8(_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1, L_12, NULL);
		NullCheck((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9);
		TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9, L_13, TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		V_5 = L_9;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_14 = V_5;
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_15 = V_4;
		Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* L_16 = (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A*)il2cpp_codegen_object_new(Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859(L_16, (RuntimeObject*)L_15, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 17)), NULL);
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_17 = V_4;
		Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* L_18 = (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7*)il2cpp_codegen_object_new(Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71(L_18, (RuntimeObject*)L_17, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 18)), NULL);
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_19 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_20 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_20, (RuntimeObject*)L_19, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 19)), NULL);
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_21 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_22 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_22, (RuntimeObject*)L_21, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 20)), NULL);
		DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A((TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2*)L_14, L_16, L_18, L_20, L_22, DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_23 = V_5;
		U3CU3Ec__DisplayClass6_0_t85CFBEC443BE6BE75937E908F0BB2D394A7B0DDE* L_24 = V_4;
		EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07* L_25 = (EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07*)il2cpp_codegen_object_new(EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		EventCallback_1__ctor_m419A2A0D4A537D68FC86F3EBA2998FB37CA6F42E(L_25, (RuntimeObject*)L_24, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 21)), NULL);
		NullCheck((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23);
		CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23, L_25, (int32_t)0, CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_26 = V_5;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26);
		VirtualActionInvoker1< bool >::Invoke(17, (Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26, (bool)1);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_27 = V_5;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_28 = V_2;
		int32_t L_29 = V_3;
		NullCheck(L_28);
		int32_t L_30 = L_29;
		String_t* L_31 = (L_28)->GetAt(static_cast<il2cpp_array_size_t>(L_30));
		NullCheck((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27);
		BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27, L_31, BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_32 = V_5;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_33;
		L_33 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_34 = V_2;
		int32_t L_35 = V_3;
		NullCheck(L_34);
		int32_t L_36 = L_35;
		String_t* L_37 = (L_34)->GetAt(static_cast<il2cpp_array_size_t>(L_36));
		String_t* L_38;
		L_38 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(L_33, _stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745, L_37, NULL);
		NullCheck((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32);
		VisualElement_set_tooltip_mD392FEBD1C4F97E4FEE2FEE3E92937D355B7FA61((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32, L_38, NULL);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_39 = V_1;
		int32_t L_40 = V_3;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_41 = V_5;
		NullCheck(L_39);
		ArrayElementTypeCheck (L_39, L_41);
		(L_39)->SetAt(static_cast<il2cpp_array_size_t>(L_40), (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)L_41);
		int32_t L_42 = V_3;
		V_3 = ((int32_t)il2cpp_codegen_add(L_42, 1));
	}

IL_00da:
	{
		int32_t L_43 = V_3;
		int32_t L_44 = V_0;
		if ((((int32_t)L_43) < ((int32_t)L_44)))
		{
			goto IL_001c;
		}
	}
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_45 = V_1;
		return L_45;
	}
}
// Method Definition Index: 22743
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_m743A6409D098F26A091B4EAF78749892CBA3F021_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 V_0;
	memset((&V_0), 0, sizeof(V_0));
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		NullCheck((Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*)__this);
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_0;
		L_0 = Field_1_GetValue_m35BC0454EB241EF390E1AC4F4E9FFDF5286BDCA1((Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 22));
		V_0 = L_0;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_1 = V_0;
		int32_t L_2 = ___0_componentIndex;
		float L_3 = ___1_newValue;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_4;
		L_4 = VirtualFuncInvoker3< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7, int32_t, float >::Invoke(26, __this, L_1, L_2, L_3);
		V_1 = L_4;
		Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 L_5 = V_1;
		NullCheck((Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*)__this);
		VirtualActionInvoker1< Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 >::Invoke(20, (Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*)__this, L_5);
		return;
	}
}
// Method Definition Index: 22744
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnDecrement_m4E46FBAA34488EFE5E0512F53B337E6D04B13BF3_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = (-(std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) >= ((float)((float)il2cpp_codegen_add(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_subtract(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		VectorField_1_UpdateVectorComponent_m743A6409D098F26A091B4EAF78749892CBA3F021(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22745
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnIncrement_mFDCF21E8EBE312CAEAEFA043AD3C173C307F0694_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = ((std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) <= ((float)((float)il2cpp_codegen_subtract(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_add(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		VectorField_1_UpdateVectorComponent_m743A6409D098F26A091B4EAF78749892CBA3F021(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22753
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1__ctor_m8F08DF879EDA1A7B020FEA890F1CBC10AB6232D7_gshared (VectorField_1_tA0DD3460E58AEABD9D5B33110FC64F4490179B87* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		__this->___incStep = (0.0250000004f);
		__this->___incStepMult = (10.0f);
		__this->___decimals = 3;
		__this->___selectedComponent = (-1);
		Field_1__ctor_m7C303BA68691F2521E9EC689B23AE797B4DFBB05((Field_1_tA072783C26CACD3E84F9B62900C79E98AA01B8ED*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 28));
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 22740
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* VectorField_1_Create_m5F01418EEBAFDA6602AD44E1660256C879710491_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* V_0 = NULL;
	{
		int32_t L_0 = ((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this)->___m_Context;
		bool L_1;
		L_1 = DebugUIExtensions_IsAnyRuntimeContext_mF1F32BD66156D14BC1CD24234D8EEAAE324EFB5D(L_0, NULL);
		if (!L_1)
		{
			goto IL_0016;
		}
	}
	{
		BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* L_2;
		L_2 = VectorField_1_CreateRuntimeVectorField_m086D519FFD86271A500C37EECA4F5736D8A18CE4(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		V_0 = L_2;
		goto IL_001d;
	}

IL_0016:
	{
		BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* L_3;
		L_3 = VirtualFuncInvoker0< BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* >::Invoke(27, __this);
		V_0 = L_3;
	}

IL_001d:
	{
		BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* L_4 = V_0;
		BaseFieldHelper_ConfigureBaseField_TisVector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_mEABD641224D190E163CBC2A813D1E24148F1DAA3((Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*)__this, L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 4));
		BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* L_5 = V_0;
		return (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_5;
	}
}
// Method Definition Index: 22741
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B* VectorField_1_CreateRuntimeVectorField_m086D519FFD86271A500C37EECA4F5736D8A18CE4_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral34761125773C5996C26093DBBA81463705909069);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* V_0 = NULL;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	int32_t V_2 = 0;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_3 = NULL;
	{
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_0 = (U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6));
		U3CU3Ec__DisplayClass5_0__ctor_mB45641A30C6F56A00296C6AA5FC2E88108F6D8E9(L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		V_0 = L_0;
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_1 = V_0;
		NullCheck(L_1);
		L_1->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_1->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_2 = V_0;
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_3 = (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)il2cpp_codegen_object_new(VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		VisualElement__ctor_m4C59A7BA0CE74223A61F07C39A60071DD0207E2D(L_3, NULL);
		NullCheck(L_2);
		L_2->___fieldsContainer = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&L_2->___fieldsContainer), (void*)L_3);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_4 = V_0;
		NullCheck(L_4);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_5 = L_4->___fieldsContainer;
		String_t* L_6;
		L_6 = VirtualFuncInvoker0< String_t* >::Invoke(24, __this);
		String_t* L_7;
		L_7 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146, L_6, _stringLiteral34761125773C5996C26093DBBA81463705909069, NULL);
		NullCheck(L_5);
		VisualElement_AddToClassList_mAF0DD8D8CFD5130229A0471DD260E01ED82117F1(L_5, L_7, NULL);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_8 = V_0;
		NullCheck(L_8);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_9 = L_8->___fieldsContainer;
		NullCheck(L_9);
		RuntimeObject* L_10;
		L_10 = VisualElement_get_style_mDCFF8D835BE0AFE412905E108F48B32A83734224(L_9, NULL);
		StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE L_11;
		L_11 = StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD((int32_t)0, StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		NullCheck(L_10);
		InterfaceActionInvoker1< StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE >::Invoke(57, IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var, L_10, L_11);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_12 = V_0;
		NullCheck(L_12);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_13 = L_12->___fieldsContainer;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13);
		Focusable_set_delegatesFocus_mC691C4199C88BEF0C55A7F7FD2C6ADDD00402D6F((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13, (bool)1, NULL);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_14 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_15;
		L_15 = VectorField_1_CreateComponentFields_mBA6A2A437F8CABA005347D6D7A586B0CEC510D72(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 9));
		NullCheck(L_14);
		L_14->___componentFields = L_15;
		Il2CppCodeGenWriteBarrier((void**)(&L_14->___componentFields), (void*)L_15);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_16 = V_0;
		NullCheck(L_16);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_17 = L_16->___componentFields;
		V_1 = L_17;
		V_2 = 0;
		goto IL_0085;
	}

IL_0071:
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_18 = V_1;
		int32_t L_19 = V_2;
		NullCheck(L_18);
		int32_t L_20 = L_19;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_21 = (L_18)->GetAt(static_cast<il2cpp_array_size_t>(L_20));
		V_3 = L_21;
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_22 = V_0;
		NullCheck(L_22);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_23 = L_22->___fieldsContainer;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_24 = V_3;
		NullCheck(L_23);
		VisualElement_Add_mE2571CCB23C09103F8732EEC73833683F7236A7F(L_23, (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_24, NULL);
		int32_t L_25 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_25, 1));
	}

IL_0085:
	{
		int32_t L_26 = V_2;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_27 = V_1;
		NullCheck(L_27);
		int32_t L_28 = (il2cpp_codegen_conv<int32_t,int64_t,int64_t,false,false>((((RuntimeArray*)L_27)->max_length),NULL));
		if ((((int32_t)L_26) < ((int32_t)L_28)))
		{
			goto IL_0071;
		}
	}
	{
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_29 = V_0;
		NullCheck(L_29);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_30 = L_29->___fieldsContainer;
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_31 = V_0;
		Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780* L_32 = (Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780*)il2cpp_codegen_object_new(Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		Func_1__ctor_m4D45A6927B7EBCE17611779C8DD2E5257CB11922(L_32, (RuntimeObject*)L_31, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 10)), NULL);
		DebugUIExtensions_ScheduleTracked_mC19444ADBEE6613D2D7C121BCD5B6F2E8233EDEE((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, L_30, L_32, NULL);
		U3CU3Ec__DisplayClass5_0_t36BE198B7D7A6675C2B3325887A60749B642CD45* L_33 = V_0;
		NullCheck(L_33);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_34 = L_33->___fieldsContainer;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_35;
		L_35 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7* L_36 = (RuntimeBaseFieldWrapper_1_t82AA00F7D7E7792FB5E221480DFCEB38339A29C7*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 11));
		RuntimeBaseFieldWrapper_1__ctor_m45AE15129A72C4E53C8219F3FED6CB2B8823E369(L_36, L_34, L_35, il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		return (BaseField_1_tB78B850472E8D80C410522B83653501AA27A339B*)L_36;
	}
}
// Method Definition Index: 22742
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_mBA6A2A437F8CABA005347D6D7A586B0CEC510D72_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* V_2 = NULL;
	int32_t V_3 = 0;
	U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* V_4 = NULL;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_5 = NULL;
	{
		int32_t L_0;
		L_0 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		V_0 = L_0;
		int32_t L_1 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_2 = (FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)SZArrayNew(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var, (uint32_t)L_1);
		V_1 = L_2;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_3;
		L_3 = VirtualFuncInvoker0< StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* >::Invoke(23, __this);
		V_2 = L_3;
		V_3 = 0;
		goto IL_00da;
	}

IL_001c:
	{
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_4 = (U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 15));
		U3CU3Ec__DisplayClass6_0__ctor_mB87D4FA5A89373F41711521900B5FAB81717C133(L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		V_4 = L_4;
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_5 = V_4;
		NullCheck(L_5);
		L_5->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_5->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_6 = V_4;
		int32_t L_7 = V_3;
		NullCheck(L_6);
		L_6->___componentIndex = L_7;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_8 = (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)il2cpp_codegen_object_new(FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		FloatField__ctor_m46165DF8164AD0A15CBD7FACE88D8902BE113696(L_8, NULL);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_9 = L_8;
		int32_t L_10 = __this->___decimals;
		int32_t L_11 = L_10;
		RuntimeObject* L_12 = Box(il2cpp_defaults.int32_class, &L_11);
		String_t* L_13;
		L_13 = String_Format_mA8DBB4C2516B9723C5A41E6CB1E2FAF4BBE96DD8(_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1, L_12, NULL);
		NullCheck((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9);
		TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9, L_13, TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		V_5 = L_9;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_14 = V_5;
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_15 = V_4;
		Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* L_16 = (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A*)il2cpp_codegen_object_new(Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859(L_16, (RuntimeObject*)L_15, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 17)), NULL);
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_17 = V_4;
		Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* L_18 = (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7*)il2cpp_codegen_object_new(Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71(L_18, (RuntimeObject*)L_17, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 18)), NULL);
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_19 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_20 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_20, (RuntimeObject*)L_19, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 19)), NULL);
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_21 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_22 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_22, (RuntimeObject*)L_21, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 20)), NULL);
		DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A((TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2*)L_14, L_16, L_18, L_20, L_22, DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_23 = V_5;
		U3CU3Ec__DisplayClass6_0_t43EAD049880B19B072AEF58F5289B757520ABB00* L_24 = V_4;
		EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07* L_25 = (EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07*)il2cpp_codegen_object_new(EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		EventCallback_1__ctor_m419A2A0D4A537D68FC86F3EBA2998FB37CA6F42E(L_25, (RuntimeObject*)L_24, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 21)), NULL);
		NullCheck((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23);
		CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23, L_25, (int32_t)0, CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_26 = V_5;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26);
		VirtualActionInvoker1< bool >::Invoke(17, (Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26, (bool)1);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_27 = V_5;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_28 = V_2;
		int32_t L_29 = V_3;
		NullCheck(L_28);
		int32_t L_30 = L_29;
		String_t* L_31 = (L_28)->GetAt(static_cast<il2cpp_array_size_t>(L_30));
		NullCheck((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27);
		BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27, L_31, BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_32 = V_5;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_33;
		L_33 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_34 = V_2;
		int32_t L_35 = V_3;
		NullCheck(L_34);
		int32_t L_36 = L_35;
		String_t* L_37 = (L_34)->GetAt(static_cast<il2cpp_array_size_t>(L_36));
		String_t* L_38;
		L_38 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(L_33, _stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745, L_37, NULL);
		NullCheck((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32);
		VisualElement_set_tooltip_mD392FEBD1C4F97E4FEE2FEE3E92937D355B7FA61((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32, L_38, NULL);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_39 = V_1;
		int32_t L_40 = V_3;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_41 = V_5;
		NullCheck(L_39);
		ArrayElementTypeCheck (L_39, L_41);
		(L_39)->SetAt(static_cast<il2cpp_array_size_t>(L_40), (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)L_41);
		int32_t L_42 = V_3;
		V_3 = ((int32_t)il2cpp_codegen_add(L_42, 1));
	}

IL_00da:
	{
		int32_t L_43 = V_3;
		int32_t L_44 = V_0;
		if ((((int32_t)L_43) < ((int32_t)L_44)))
		{
			goto IL_001c;
		}
	}
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_45 = V_1;
		return L_45;
	}
}
// Method Definition Index: 22743
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_mD23EF8338D2FE25735FF01F04C1A5EAE4797BD75_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 V_0;
	memset((&V_0), 0, sizeof(V_0));
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		NullCheck((Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*)__this);
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_0;
		L_0 = Field_1_GetValue_m64B831DFD988D14A1DA8AD2548F2B1D1867218CC((Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 22));
		V_0 = L_0;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_1 = V_0;
		int32_t L_2 = ___0_componentIndex;
		float L_3 = ___1_newValue;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_4;
		L_4 = VirtualFuncInvoker3< Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2, int32_t, float >::Invoke(26, __this, L_1, L_2, L_3);
		V_1 = L_4;
		Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 L_5 = V_1;
		NullCheck((Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*)__this);
		VirtualActionInvoker1< Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 >::Invoke(20, (Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*)__this, L_5);
		return;
	}
}
// Method Definition Index: 22744
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnDecrement_m369944C0E4BEA69A6F61E2F79CE4A0B8A812D485_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = (-(std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) >= ((float)((float)il2cpp_codegen_add(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_subtract(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		VectorField_1_UpdateVectorComponent_mD23EF8338D2FE25735FF01F04C1A5EAE4797BD75(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22745
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnIncrement_m12FEA7CBF602BE8748581163C2244E915009B309_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = ((std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) <= ((float)((float)il2cpp_codegen_subtract(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_add(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		VectorField_1_UpdateVectorComponent_mD23EF8338D2FE25735FF01F04C1A5EAE4797BD75(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22753
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1__ctor_m68A8E7C0642A1F49BB638A5059411FDDFB21E070_gshared (VectorField_1_t922D9F74763B4AFD1C1760DE2236972042F8310D* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		__this->___incStep = (0.0250000004f);
		__this->___incStepMult = (10.0f);
		__this->___decimals = 3;
		__this->___selectedComponent = (-1);
		Field_1__ctor_m5E85360C971446C73E1A8E5ED7DA17D7EDC90E1B((Field_1_tC3CCA8F7619A0B639B6671BD922EC68E34595E18*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 28));
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 22740
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* VectorField_1_Create_m95C75EEB02CDDBA9A30CF3D253D669132FB1CDBB_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* V_0 = NULL;
	{
		int32_t L_0 = ((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this)->___m_Context;
		bool L_1;
		L_1 = DebugUIExtensions_IsAnyRuntimeContext_mF1F32BD66156D14BC1CD24234D8EEAAE324EFB5D(L_0, NULL);
		if (!L_1)
		{
			goto IL_0016;
		}
	}
	{
		BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* L_2;
		L_2 = VectorField_1_CreateRuntimeVectorField_m8CF1C678A918D8CC2829C6A4DCA4A4527975BC4E(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		V_0 = L_2;
		goto IL_001d;
	}

IL_0016:
	{
		BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* L_3;
		L_3 = VirtualFuncInvoker0< BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* >::Invoke(27, __this);
		V_0 = L_3;
	}

IL_001d:
	{
		BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* L_4 = V_0;
		BaseFieldHelper_ConfigureBaseField_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mC7D4E12978C810E289F2F469779BF6FEC8D368EE((Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*)__this, L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 4));
		BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* L_5 = V_0;
		return (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_5;
	}
}
// Method Definition Index: 22741
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F* VectorField_1_CreateRuntimeVectorField_m8CF1C678A918D8CC2829C6A4DCA4A4527975BC4E_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral34761125773C5996C26093DBBA81463705909069);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* V_0 = NULL;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	int32_t V_2 = 0;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_3 = NULL;
	{
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_0 = (U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6));
		U3CU3Ec__DisplayClass5_0__ctor_mB73C181FA5C0498C3141BAA1FDC95F3A13FA6B46(L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		V_0 = L_0;
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_1 = V_0;
		NullCheck(L_1);
		L_1->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_1->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_2 = V_0;
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_3 = (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)il2cpp_codegen_object_new(VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		VisualElement__ctor_m4C59A7BA0CE74223A61F07C39A60071DD0207E2D(L_3, NULL);
		NullCheck(L_2);
		L_2->___fieldsContainer = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&L_2->___fieldsContainer), (void*)L_3);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_4 = V_0;
		NullCheck(L_4);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_5 = L_4->___fieldsContainer;
		String_t* L_6;
		L_6 = VirtualFuncInvoker0< String_t* >::Invoke(24, __this);
		String_t* L_7;
		L_7 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146, L_6, _stringLiteral34761125773C5996C26093DBBA81463705909069, NULL);
		NullCheck(L_5);
		VisualElement_AddToClassList_mAF0DD8D8CFD5130229A0471DD260E01ED82117F1(L_5, L_7, NULL);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_8 = V_0;
		NullCheck(L_8);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_9 = L_8->___fieldsContainer;
		NullCheck(L_9);
		RuntimeObject* L_10;
		L_10 = VisualElement_get_style_mDCFF8D835BE0AFE412905E108F48B32A83734224(L_9, NULL);
		StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE L_11;
		L_11 = StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD((int32_t)0, StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		NullCheck(L_10);
		InterfaceActionInvoker1< StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE >::Invoke(57, IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var, L_10, L_11);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_12 = V_0;
		NullCheck(L_12);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_13 = L_12->___fieldsContainer;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13);
		Focusable_set_delegatesFocus_mC691C4199C88BEF0C55A7F7FD2C6ADDD00402D6F((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13, (bool)1, NULL);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_14 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_15;
		L_15 = VectorField_1_CreateComponentFields_mE9B22CD2331602818289DC43C983A1C0CF08A160(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 9));
		NullCheck(L_14);
		L_14->___componentFields = L_15;
		Il2CppCodeGenWriteBarrier((void**)(&L_14->___componentFields), (void*)L_15);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_16 = V_0;
		NullCheck(L_16);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_17 = L_16->___componentFields;
		V_1 = L_17;
		V_2 = 0;
		goto IL_0085;
	}

IL_0071:
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_18 = V_1;
		int32_t L_19 = V_2;
		NullCheck(L_18);
		int32_t L_20 = L_19;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_21 = (L_18)->GetAt(static_cast<il2cpp_array_size_t>(L_20));
		V_3 = L_21;
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_22 = V_0;
		NullCheck(L_22);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_23 = L_22->___fieldsContainer;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_24 = V_3;
		NullCheck(L_23);
		VisualElement_Add_mE2571CCB23C09103F8732EEC73833683F7236A7F(L_23, (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_24, NULL);
		int32_t L_25 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_25, 1));
	}

IL_0085:
	{
		int32_t L_26 = V_2;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_27 = V_1;
		NullCheck(L_27);
		int32_t L_28 = (il2cpp_codegen_conv<int32_t,int64_t,int64_t,false,false>((((RuntimeArray*)L_27)->max_length),NULL));
		if ((((int32_t)L_26) < ((int32_t)L_28)))
		{
			goto IL_0071;
		}
	}
	{
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_29 = V_0;
		NullCheck(L_29);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_30 = L_29->___fieldsContainer;
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_31 = V_0;
		Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780* L_32 = (Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780*)il2cpp_codegen_object_new(Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		Func_1__ctor_m4D45A6927B7EBCE17611779C8DD2E5257CB11922(L_32, (RuntimeObject*)L_31, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 10)), NULL);
		DebugUIExtensions_ScheduleTracked_mC19444ADBEE6613D2D7C121BCD5B6F2E8233EDEE((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, L_30, L_32, NULL);
		U3CU3Ec__DisplayClass5_0_t467A1816C2DFA1364975C5A1C919AB6FE9267807* L_33 = V_0;
		NullCheck(L_33);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_34 = L_33->___fieldsContainer;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_35;
		L_35 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7* L_36 = (RuntimeBaseFieldWrapper_1_t339EC09B6A93B0CE0BE64ED5E2A48230C76948B7*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 11));
		RuntimeBaseFieldWrapper_1__ctor_mB2BF8A0896212028CB08FE015B29DD4655D58113(L_36, L_34, L_35, il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		return (BaseField_1_tA1F41315EF3CC62659FC6306606C7CCA98D85C9F*)L_36;
	}
}
// Method Definition Index: 22742
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_mE9B22CD2331602818289DC43C983A1C0CF08A160_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* V_2 = NULL;
	int32_t V_3 = 0;
	U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* V_4 = NULL;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_5 = NULL;
	{
		int32_t L_0;
		L_0 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		V_0 = L_0;
		int32_t L_1 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_2 = (FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)SZArrayNew(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var, (uint32_t)L_1);
		V_1 = L_2;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_3;
		L_3 = VirtualFuncInvoker0< StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* >::Invoke(23, __this);
		V_2 = L_3;
		V_3 = 0;
		goto IL_00da;
	}

IL_001c:
	{
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_4 = (U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 15));
		U3CU3Ec__DisplayClass6_0__ctor_m3809B224386D2FB43C8E6BFDE9FDE0AD6E91B5F3(L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		V_4 = L_4;
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_5 = V_4;
		NullCheck(L_5);
		L_5->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_5->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_6 = V_4;
		int32_t L_7 = V_3;
		NullCheck(L_6);
		L_6->___componentIndex = L_7;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_8 = (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)il2cpp_codegen_object_new(FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		FloatField__ctor_m46165DF8164AD0A15CBD7FACE88D8902BE113696(L_8, NULL);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_9 = L_8;
		int32_t L_10 = __this->___decimals;
		int32_t L_11 = L_10;
		RuntimeObject* L_12 = Box(il2cpp_defaults.int32_class, &L_11);
		String_t* L_13;
		L_13 = String_Format_mA8DBB4C2516B9723C5A41E6CB1E2FAF4BBE96DD8(_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1, L_12, NULL);
		NullCheck((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9);
		TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9, L_13, TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		V_5 = L_9;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_14 = V_5;
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_15 = V_4;
		Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* L_16 = (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A*)il2cpp_codegen_object_new(Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859(L_16, (RuntimeObject*)L_15, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 17)), NULL);
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_17 = V_4;
		Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* L_18 = (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7*)il2cpp_codegen_object_new(Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71(L_18, (RuntimeObject*)L_17, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 18)), NULL);
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_19 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_20 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_20, (RuntimeObject*)L_19, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 19)), NULL);
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_21 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_22 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_22, (RuntimeObject*)L_21, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 20)), NULL);
		DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A((TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2*)L_14, L_16, L_18, L_20, L_22, DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_23 = V_5;
		U3CU3Ec__DisplayClass6_0_tB885916F2DDE668D1EA8BE7AD93DEF09455BACE5* L_24 = V_4;
		EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07* L_25 = (EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07*)il2cpp_codegen_object_new(EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		EventCallback_1__ctor_m419A2A0D4A537D68FC86F3EBA2998FB37CA6F42E(L_25, (RuntimeObject*)L_24, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 21)), NULL);
		NullCheck((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23);
		CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23, L_25, (int32_t)0, CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_26 = V_5;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26);
		VirtualActionInvoker1< bool >::Invoke(17, (Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26, (bool)1);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_27 = V_5;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_28 = V_2;
		int32_t L_29 = V_3;
		NullCheck(L_28);
		int32_t L_30 = L_29;
		String_t* L_31 = (L_28)->GetAt(static_cast<il2cpp_array_size_t>(L_30));
		NullCheck((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27);
		BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27, L_31, BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_32 = V_5;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_33;
		L_33 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_34 = V_2;
		int32_t L_35 = V_3;
		NullCheck(L_34);
		int32_t L_36 = L_35;
		String_t* L_37 = (L_34)->GetAt(static_cast<il2cpp_array_size_t>(L_36));
		String_t* L_38;
		L_38 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(L_33, _stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745, L_37, NULL);
		NullCheck((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32);
		VisualElement_set_tooltip_mD392FEBD1C4F97E4FEE2FEE3E92937D355B7FA61((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32, L_38, NULL);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_39 = V_1;
		int32_t L_40 = V_3;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_41 = V_5;
		NullCheck(L_39);
		ArrayElementTypeCheck (L_39, L_41);
		(L_39)->SetAt(static_cast<il2cpp_array_size_t>(L_40), (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)L_41);
		int32_t L_42 = V_3;
		V_3 = ((int32_t)il2cpp_codegen_add(L_42, 1));
	}

IL_00da:
	{
		int32_t L_43 = V_3;
		int32_t L_44 = V_0;
		if ((((int32_t)L_43) < ((int32_t)L_44)))
		{
			goto IL_001c;
		}
	}
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_45 = V_1;
		return L_45;
	}
}
// Method Definition Index: 22743
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_mC184D222EEF986D89732893626E7FD35F5611DB3_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 V_0;
	memset((&V_0), 0, sizeof(V_0));
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		NullCheck((Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*)__this);
		Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 L_0;
		L_0 = Field_1_GetValue_m36C6FCEF8084FFAC4351BB53A298145B627A4CA6((Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 22));
		V_0 = L_0;
		Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 L_1 = V_0;
		int32_t L_2 = ___0_componentIndex;
		float L_3 = ___1_newValue;
		Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 L_4;
		L_4 = VirtualFuncInvoker3< Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3, int32_t, float >::Invoke(26, __this, L_1, L_2, L_3);
		V_1 = L_4;
		Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 L_5 = V_1;
		NullCheck((Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*)__this);
		VirtualActionInvoker1< Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 >::Invoke(20, (Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*)__this, L_5);
		return;
	}
}
// Method Definition Index: 22744
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnDecrement_m5B4F8AA874FD7205A4014650A4235BD339687AB6_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = (-(std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) >= ((float)((float)il2cpp_codegen_add(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_subtract(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		VectorField_1_UpdateVectorComponent_mC184D222EEF986D89732893626E7FD35F5611DB3(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22745
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnIncrement_mC3563C75F4B59E6749F1BB6BF6FE76B10D53DB36_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = ((std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) <= ((float)((float)il2cpp_codegen_subtract(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_add(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		VectorField_1_UpdateVectorComponent_mC184D222EEF986D89732893626E7FD35F5611DB3(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22753
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1__ctor_mA147DDFF53B038A9BAC6EAEEBE45ED829C91DB95_gshared (VectorField_1_t7640EEE30580F0D8ABCA05DBBAB2F6B83A4713C3* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		__this->___incStep = (0.0250000004f);
		__this->___incStepMult = (10.0f);
		__this->___decimals = 3;
		__this->___selectedComponent = (-1);
		Field_1__ctor_m987712BF4E8BBF11473DA83B4CF70877C002430C((Field_1_t13BBC583A7E521A9A0C5B9A2B8B537D8CEE550BD*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 28));
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 22740
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* VectorField_1_Create_m6D90F2CDEE426C36FC579CD7C3AAF60C54FC5942_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* V_0 = NULL;
	{
		int32_t L_0 = ((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this)->___m_Context;
		bool L_1;
		L_1 = DebugUIExtensions_IsAnyRuntimeContext_mF1F32BD66156D14BC1CD24234D8EEAAE324EFB5D(L_0, NULL);
		if (!L_1)
		{
			goto IL_0016;
		}
	}
	{
		BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* L_2;
		L_2 = ((  BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* (*) (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 0)))(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		V_0 = L_2;
		goto IL_001d;
	}

IL_0016:
	{
		BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* L_3;
		L_3 = VirtualFuncInvoker0< BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* >::Invoke(27, __this);
		V_0 = L_3;
	}

IL_001d:
	{
		BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* L_4 = V_0;
		((  void (*) (Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*, BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 4)))((Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*)__this, L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 4));
		BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* L_5 = V_0;
		return (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_5;
	}
}
// Method Definition Index: 22741
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E* VectorField_1_CreateRuntimeVectorField_mA41774C8180AE0E77806DE4C9535F8053D5E3260_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral34761125773C5996C26093DBBA81463705909069);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* V_0 = NULL;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	int32_t V_2 = 0;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_3 = NULL;
	{
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_0 = (U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6));
		((  void (*) (U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 7)))(L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		V_0 = L_0;
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_1 = V_0;
		NullCheck(L_1);
		L_1->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_1->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_2 = V_0;
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_3 = (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)il2cpp_codegen_object_new(VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115_il2cpp_TypeInfo_var);
		VisualElement__ctor_m4C59A7BA0CE74223A61F07C39A60071DD0207E2D(L_3, NULL);
		NullCheck(L_2);
		L_2->___fieldsContainer = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&L_2->___fieldsContainer), (void*)L_3);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_4 = V_0;
		NullCheck(L_4);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_5 = L_4->___fieldsContainer;
		String_t* L_6;
		L_6 = VirtualFuncInvoker0< String_t* >::Invoke(24, __this);
		String_t* L_7;
		L_7 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(_stringLiteral2CA2C8807434E0CAD4DC230E16C157BD72322146, L_6, _stringLiteral34761125773C5996C26093DBBA81463705909069, NULL);
		NullCheck(L_5);
		VisualElement_AddToClassList_mAF0DD8D8CFD5130229A0471DD260E01ED82117F1(L_5, L_7, NULL);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_8 = V_0;
		NullCheck(L_8);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_9 = L_8->___fieldsContainer;
		NullCheck(L_9);
		RuntimeObject* L_10;
		L_10 = VisualElement_get_style_mDCFF8D835BE0AFE412905E108F48B32A83734224(L_9, NULL);
		StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE L_11;
		L_11 = StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD((int32_t)0, StyleEnum_1_op_Implicit_m9CC7BE6DFC463FD482DC9D6E3A496FCD0017FCCD_RuntimeMethod_var);
		NullCheck(L_10);
		InterfaceActionInvoker1< StyleEnum_1_t4C47F320FF81E91A50EC2AD0D70A3D620362BBAE >::Invoke(57, IStyle_t4FD66C97CA5F46BFE328FED0C65277A37E0A89F7_il2cpp_TypeInfo_var, L_10, L_11);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_12 = V_0;
		NullCheck(L_12);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_13 = L_12->___fieldsContainer;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13);
		Focusable_set_delegatesFocus_mC691C4199C88BEF0C55A7F7FD2C6ADDD00402D6F((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_13, (bool)1, NULL);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_14 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_15;
		L_15 = ((  FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* (*) (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 9)))(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 9));
		NullCheck(L_14);
		L_14->___componentFields = L_15;
		Il2CppCodeGenWriteBarrier((void**)(&L_14->___componentFields), (void*)L_15);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_16 = V_0;
		NullCheck(L_16);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_17 = L_16->___componentFields;
		V_1 = L_17;
		V_2 = 0;
		goto IL_0085;
	}

IL_0071:
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_18 = V_1;
		int32_t L_19 = V_2;
		NullCheck(L_18);
		int32_t L_20 = L_19;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_21 = (L_18)->GetAt(static_cast<il2cpp_array_size_t>(L_20));
		V_3 = L_21;
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_22 = V_0;
		NullCheck(L_22);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_23 = L_22->___fieldsContainer;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_24 = V_3;
		NullCheck(L_23);
		VisualElement_Add_mE2571CCB23C09103F8732EEC73833683F7236A7F(L_23, (VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_24, NULL);
		int32_t L_25 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_25, 1));
	}

IL_0085:
	{
		int32_t L_26 = V_2;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_27 = V_1;
		NullCheck(L_27);
		int32_t L_28 = (il2cpp_codegen_conv<int32_t,int64_t,int64_t,false,false>((((RuntimeArray*)L_27)->max_length),NULL));
		if ((((int32_t)L_26) < ((int32_t)L_28)))
		{
			goto IL_0071;
		}
	}
	{
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_29 = V_0;
		NullCheck(L_29);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_30 = L_29->___fieldsContainer;
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_31 = V_0;
		Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780* L_32 = (Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780*)il2cpp_codegen_object_new(Func_1_t37FC88D0B5B5C9C21F4EA505421F3F2ED52B0780_il2cpp_TypeInfo_var);
		Func_1__ctor_m4D45A6927B7EBCE17611779C8DD2E5257CB11922(L_32, (RuntimeObject*)L_31, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 10)), NULL);
		DebugUIExtensions_ScheduleTracked_mC19444ADBEE6613D2D7C121BCD5B6F2E8233EDEE((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, L_30, L_32, NULL);
		U3CU3Ec__DisplayClass5_0_t29266D6097056C33D4C85ED3F9DFDFFCA8E58553* L_33 = V_0;
		NullCheck(L_33);
		VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115* L_34 = L_33->___fieldsContainer;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_35;
		L_35 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		RuntimeBaseFieldWrapper_1_t8D205E3CBF43B1E5DB0FA8F4A4914FE9FEF31653* L_36 = (RuntimeBaseFieldWrapper_1_t8D205E3CBF43B1E5DB0FA8F4A4914FE9FEF31653*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 11));
		((  void (*) (RuntimeBaseFieldWrapper_1_t8D205E3CBF43B1E5DB0FA8F4A4914FE9FEF31653*, VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*, String_t*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 12)))(L_36, L_34, L_35, il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		return (BaseField_1_t0E7F7564F1B6E71E8FA500F326D166535D2DC01E*)L_36;
	}
}
// Method Definition Index: 22742
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* VectorField_1_CreateComponentFields_m71D0ADC3C3D93858A443F8D0660C26F5D30A1F98_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* V_1 = NULL;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* V_2 = NULL;
	int32_t V_3 = 0;
	U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* V_4 = NULL;
	FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* V_5 = NULL;
	{
		int32_t L_0;
		L_0 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		V_0 = L_0;
		int32_t L_1 = V_0;
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_2 = (FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F*)SZArrayNew(FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F_il2cpp_TypeInfo_var, (uint32_t)L_1);
		V_1 = L_2;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_3;
		L_3 = VirtualFuncInvoker0< StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* >::Invoke(23, __this);
		V_2 = L_3;
		V_3 = 0;
		goto IL_00da;
	}

IL_001c:
	{
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_4 = (U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6*)il2cpp_codegen_object_new(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 15));
		((  void (*) (U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 16)))(L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		V_4 = L_4;
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_5 = V_4;
		NullCheck(L_5);
		L_5->___U3CU3E4__this = __this;
		Il2CppCodeGenWriteBarrier((void**)(&L_5->___U3CU3E4__this), (void*)__this);
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_6 = V_4;
		int32_t L_7 = V_3;
		NullCheck(L_6);
		L_6->___componentIndex = L_7;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_8 = (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)il2cpp_codegen_object_new(FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0_il2cpp_TypeInfo_var);
		FloatField__ctor_m46165DF8164AD0A15CBD7FACE88D8902BE113696(L_8, NULL);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_9 = L_8;
		int32_t L_10 = __this->___decimals;
		int32_t L_11 = L_10;
		RuntimeObject* L_12 = Box(il2cpp_defaults.int32_class, &L_11);
		String_t* L_13;
		L_13 = String_Format_mA8DBB4C2516B9723C5A41E6CB1E2FAF4BBE96DD8(_stringLiteral0C7CFF6DF407D7A4C669EEA76EB758FEADC433B1, L_12, NULL);
		NullCheck((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9);
		TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552((TextValueField_1_t577850A2F4D172CB194B508EF12AC0D2096EA87B*)L_9, L_13, TextValueField_1_set_formatString_m01603A22670906EBBBAA5380174B3D293751E552_RuntimeMethod_var);
		V_5 = L_9;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_14 = V_5;
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_15 = V_4;
		Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A* L_16 = (Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A*)il2cpp_codegen_object_new(Action_1_t310F18CB4338A2740CA701F160C62E2C3198E66A_il2cpp_TypeInfo_var);
		Action_1__ctor_m770CD2F8BB65F2EDA5128CA2F96D71C35B23E859(L_16, (RuntimeObject*)L_15, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 17)), NULL);
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_17 = V_4;
		Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7* L_18 = (Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7*)il2cpp_codegen_object_new(Func_1_t11C3830580201E0A3A3A2C76523B7905EF40BCF7_il2cpp_TypeInfo_var);
		Func_1__ctor_m94E0C712F86101E026834DA318C3FFF46DE49C71(L_18, (RuntimeObject*)L_17, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 18)), NULL);
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_19 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_20 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_20, (RuntimeObject*)L_19, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 19)), NULL);
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_21 = V_4;
		Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C* L_22 = (Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C*)il2cpp_codegen_object_new(Action_1_t10DCB0C07D0D3C565CEACADC80D1152B35A45F6C_il2cpp_TypeInfo_var);
		Action_1__ctor_mA8C3AC97D1F076EA5D1D0C10CEE6BD3E94711501(L_22, (RuntimeObject*)L_21, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 20)), NULL);
		DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A((TextInputBaseField_1_tC693C9223BC3F56D9387F26A86CAB402209921C2*)L_14, L_16, L_18, L_20, L_22, DebugUIStepperHelper_AddStepper_TisSingle_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C_mECCA0CAEFE7EFF1F46FA41990CBA65D197A2114A_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_23 = V_5;
		U3CU3Ec__DisplayClass6_0_t1FEC0D6659A98E511B94BA49913D40B5500153E6* L_24 = V_4;
		EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07* L_25 = (EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07*)il2cpp_codegen_object_new(EventCallback_1_t927E299F0ACAE553E3F8976037FE92354F5BAE07_il2cpp_TypeInfo_var);
		EventCallback_1__ctor_m419A2A0D4A537D68FC86F3EBA2998FB37CA6F42E(L_25, (RuntimeObject*)L_24, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 21)), NULL);
		NullCheck((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23);
		CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED((CallbackEventHandler_t99E35735225B4ACEAD1BA981632FD2D46E9CB2B4*)L_23, L_25, (int32_t)0, CallbackEventHandler_RegisterCallback_TisFocusInEvent_tC1179D309FE58E4766E94687240513A96F08AFBE_m40BE00C566B603392A8CBE51E655BA0E4E1A34ED_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_26 = V_5;
		NullCheck((Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26);
		VirtualActionInvoker1< bool >::Invoke(17, (Focusable_t39F2BAF0AF6CA465BC2BEDAF9B5B2CF379B846D0*)L_26, (bool)1);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_27 = V_5;
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_28 = V_2;
		int32_t L_29 = V_3;
		NullCheck(L_28);
		int32_t L_30 = L_29;
		String_t* L_31 = (L_28)->GetAt(static_cast<il2cpp_array_size_t>(L_30));
		NullCheck((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27);
		BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F((BaseField_1_t1BD309672FAB87887168305A09C320E26F166DA7*)L_27, L_31, BaseField_1_set_label_m105B66DF1D892BBDFE92C5ECCDA9450BBB763A6F_RuntimeMethod_var);
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_32 = V_5;
		NullCheck((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this);
		String_t* L_33;
		L_33 = Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline((Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF*)__this, NULL);
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_34 = V_2;
		int32_t L_35 = V_3;
		NullCheck(L_34);
		int32_t L_36 = L_35;
		String_t* L_37 = (L_34)->GetAt(static_cast<il2cpp_array_size_t>(L_36));
		String_t* L_38;
		L_38 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(L_33, _stringLiteral2386E77CF610F786B06A91AF2C1B3FD2282D2745, L_37, NULL);
		NullCheck((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32);
		VisualElement_set_tooltip_mD392FEBD1C4F97E4FEE2FEE3E92937D355B7FA61((VisualElement_t2667F9D19E62C7A315927506C06F223AB9234115*)L_32, L_38, NULL);
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_39 = V_1;
		int32_t L_40 = V_3;
		FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0* L_41 = V_5;
		NullCheck(L_39);
		ArrayElementTypeCheck (L_39, L_41);
		(L_39)->SetAt(static_cast<il2cpp_array_size_t>(L_40), (FloatField_tF14A4F89E4A0D2CD50B7CE4F377CA4F86B38B4E0*)L_41);
		int32_t L_42 = V_3;
		V_3 = ((int32_t)il2cpp_codegen_add(L_42, 1));
	}

IL_00da:
	{
		int32_t L_43 = V_3;
		int32_t L_44 = V_0;
		if ((((int32_t)L_43) < ((int32_t)L_44)))
		{
			goto IL_001c;
		}
	}
	{
		FloatFieldU5BU5D_tAAB4E4B057C3C08D7EF1F984AFA4E8D7D2F9471F* L_45 = V_1;
		return L_45;
	}
}
// Method Definition Index: 22743
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_UpdateVectorComponent_m8BC3B7AA6FE8D879A33C0AB95088DE3D67F8FEA1_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, int32_t ___0_componentIndex, float ___1_newValue, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 23));
	const Il2CppFullySharedGenericStruct L_0 = alloca(SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	const Il2CppFullySharedGenericStruct L_1 = alloca(SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	const Il2CppFullySharedGenericStruct L_4 = alloca(SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	const Il2CppFullySharedGenericStruct L_5 = alloca(SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	//<source_info:<no-source>:1>
	Il2CppFullySharedGenericStruct V_0 = alloca(SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	memset(V_0, 0, SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	Il2CppFullySharedGenericStruct V_1 = alloca(SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	memset(V_1, 0, SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
	{
		NullCheck((Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*)__this);
		InvokerActionInvoker1< Il2CppFullySharedGenericStruct* >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 22)), il2cpp_rgctx_method(method->klass->rgctx_data, 22), (Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*)__this, (Il2CppFullySharedGenericStruct*)L_0);
		il2cpp_codegen_memcpy(V_0, L_0, SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
		il2cpp_codegen_memcpy(L_1, V_0, SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
		int32_t L_2 = ___0_componentIndex;
		float L_3 = ___1_newValue;
		VirtualActionInvoker4Invoker< Il2CppFullySharedGenericStruct, int32_t, float, Il2CppFullySharedGenericStruct* >::Invoke(26, __this, L_1, L_2, L_3, (Il2CppFullySharedGenericStruct*)L_4);
		il2cpp_codegen_memcpy(V_1, L_4, SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
		il2cpp_codegen_memcpy(L_5, V_1, SizeOf_T_tD365A73BC40297D061AB260C2A5EEA26776A4BB9);
		NullCheck((Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*)__this);
		VirtualActionInvoker1Invoker< Il2CppFullySharedGenericStruct >::Invoke(20, (Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*)__this, L_5);
		return;
	}
}
// Method Definition Index: 22744
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnDecrement_m3BF07101061F03C44FA269D5C41E43BA01B86AF8_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = (-(std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) >= ((float)((float)il2cpp_codegen_add(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_subtract(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		((  void (*) (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F*, int32_t, float, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 27)))(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22745
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1_OnIncrement_m695CAA8A58CCF24BACB0391E7D0566AAE3D1B03D_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, bool ___0_fast, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	float V_0 = 0.0f;
	float V_1 = 0.0f;
	float V_2 = 0.0f;
	float V_3 = 0.0f;
	float G_B5_0 = 0.0f;
	float G_B8_0 = 0.0f;
	{
		int32_t L_0 = __this->___selectedComponent;
		if ((((int32_t)L_0) < ((int32_t)0)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_1 = __this->___selectedComponent;
		int32_t L_2;
		L_2 = VirtualFuncInvoker0< int32_t >::Invoke(22, __this);
		if ((((int32_t)L_1) >= ((int32_t)L_2)))
		{
			goto IL_006b;
		}
	}
	{
		int32_t L_3 = __this->___selectedComponent;
		float L_4;
		L_4 = VirtualFuncInvoker1< float, int32_t >::Invoke(21, __this, L_3);
		V_0 = L_4;
		bool L_5 = ___0_fast;
		if (L_5)
		{
			goto IL_002f;
		}
	}
	{
		float L_6 = __this->___incStep;
		G_B5_0 = L_6;
		goto IL_0035;
	}

IL_002f:
	{
		float L_7 = __this->___incStepMult;
		G_B5_0 = L_7;
	}

IL_0035:
	{
		V_1 = G_B5_0;
		V_2 = ((std::numeric_limits<float>::max)());
		float L_8 = V_0;
		float L_9 = V_2;
		float L_10 = V_1;
		if ((((float)L_8) <= ((float)((float)il2cpp_codegen_subtract(L_9, L_10)))))
		{
			goto IL_0045;
		}
	}
	{
		float L_11 = V_2;
		G_B8_0 = L_11;
		goto IL_0048;
	}

IL_0045:
	{
		float L_12 = V_0;
		float L_13 = V_1;
		G_B8_0 = ((float)il2cpp_codegen_add(L_12, L_13));
	}

IL_0048:
	{
		V_3 = G_B8_0;
		float L_14 = V_3;
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_15 = (SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C*)SZArrayNew(SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C_il2cpp_TypeInfo_var, (uint32_t)2);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_16 = L_15;
		float L_17 = V_0;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(0), (float)L_17);
		SingleU5BU5D_t89DEFE97BCEDB5857010E79ECE0F52CF6E93B87C* L_18 = L_16;
		float L_19 = V_1;
		NullCheck(L_18);
		(L_18)->SetAt(static_cast<il2cpp_array_size_t>(1), (float)L_19);
		float L_20;
		L_20 = DebugUIStepperHelper_RoundToPrecision_mE336F7C2CCB66542E916F0C21F0426FC520FACF9(L_14, L_18, NULL);
		V_3 = L_20;
		int32_t L_21 = __this->___selectedComponent;
		float L_22 = V_3;
		((  void (*) (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F*, int32_t, float, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 27)))(__this, L_21, L_22, il2cpp_rgctx_method(method->klass->rgctx_data, 27));
	}

IL_006b:
	{
		return;
	}
}
// Method Definition Index: 22753
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VectorField_1__ctor_m8C9A6BE32F096BDD676D758F25D7747CBABBA6BE_gshared (VectorField_1_tA0B76D2246CE6687E43856049B2DB46975532D1F* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		__this->___incStep = (0.0250000004f);
		__this->___incStepMult = (10.0f);
		__this->___decimals = 3;
		__this->___selectedComponent = (-1);
		((  void (*) (Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 28)))((Field_1_tEBDBEF6C7E8EC7F1DBE1ABC4B1EA917269E20258*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 28));
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 7871
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_0 = ((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_count;
		return L_0;
	}
}
// Method Definition Index: 7872
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 Vector_1_get_Zero_mDB2680DF070B1C7F273400548848C621F96343A4_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_0 = ((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_zero;
		return L_0;
	}
}
// Method Definition Index: 7873
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_InitializeCount_m43BBDDA05FDAB290038584331DB79CB33C523B83_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	VectorSizeHelper_tC26CEAD1B0D88F765A24D653A74900C4C7FEBD18 V_0;
	memset((&V_0), 0, sizeof(V_0));
	uint8_t* V_1 = NULL;
	int32_t V_2 = 0;
	int32_t G_B2_0 = 0;
	int32_t G_B21_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B5_0 = 0;
	{
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* L_0 = (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*)(&(&V_0)->____placeholder);
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_1 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&L_0->___register);
		uint8_t* L_2 = (uint8_t*)(&L_1->___byte_0);
		uintptr_t L_3 = (il2cpp_codegen_conv<uintptr_t,uint8_t*,intptr_t,false,false>(L_2,NULL));
		V_1 = (uint8_t*)L_3;
		uint8_t* L_4 = (uint8_t*)(&(&V_0)->____byte);
		uintptr_t L_5 = (il2cpp_codegen_conv<uintptr_t,uint8_t*,intptr_t,false,false>(L_4,NULL));
		uint8_t* L_6 = V_1;
		int64_t L_7 = (il2cpp_codegen_conv<int64_t,uint8_t*,intptr_t,false,false>(((uint8_t*)((intptr_t)((uint8_t*)il2cpp_codegen_subtract((intptr_t)L_5, (intptr_t)L_6))/1)),NULL));
		int32_t L_8 = (il2cpp_codegen_conv<int32_t,int64_t,int64_t,false,false>(L_7,NULL));
		V_2 = (-1);
		G_B2_0 = L_8;
		goto IL_0045;
	}

IL_0045:
	{
		G_B4_0 = G_B2_0;
		goto IL_0067;
	}

IL_0067:
	{
		G_B5_0 = G_B4_0;
	}
	{
		V_2 = 2;
		G_B21_0 = G_B5_0;
		goto IL_0176;
	}

IL_0176:
	{
		int32_t L_9 = V_2;
		return ((int32_t)(G_B21_0/L_9));
	}
}
// Method Definition Index: 7874
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, uint16_t ___0_value, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	uint8_t* V_1 = NULL;
	int32_t V_2 = 0;
	int8_t* V_3 = NULL;
	int8_t* V_4 = NULL;
	int32_t V_5 = 0;
	uint16_t* V_6 = NULL;
	uint16_t* V_7 = NULL;
	int32_t V_8 = 0;
	int16_t* V_9 = NULL;
	int16_t* V_10 = NULL;
	int32_t V_11 = 0;
	uint32_t* V_12 = NULL;
	uint32_t* V_13 = NULL;
	int32_t V_14 = 0;
	int32_t* V_15 = NULL;
	int32_t* V_16 = NULL;
	int32_t V_17 = 0;
	uint64_t* V_18 = NULL;
	uint64_t* V_19 = NULL;
	int32_t V_20 = 0;
	int64_t* V_21 = NULL;
	int64_t* V_22 = NULL;
	int32_t V_23 = 0;
	float* V_24 = NULL;
	float* V_25 = NULL;
	int32_t V_26 = 0;
	double* V_27 = NULL;
	double* V_28 = NULL;
	int32_t V_29 = 0;
	{
		il2cpp_codegen_initobj(__this, sizeof(Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489));
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_0386;
		}
	}
	{
		goto IL_005e;
	}

IL_005e:
	{
		goto IL_00b3;
	}

IL_00b3:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_1 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_2 = (uint16_t*)(&L_1->___uint16_0);
		V_7 = L_2;
		uint16_t* L_3 = V_7;
		uintptr_t L_4 = (il2cpp_codegen_conv<uintptr_t,uint16_t*,intptr_t,false,false>(L_3,NULL));
		V_6 = (uint16_t*)L_4;
		V_8 = 0;
		goto IL_00ff;
	}

IL_00e5:
	{
		uint16_t* L_5 = V_6;
		int32_t L_6 = V_8;
		intptr_t L_7 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_6,NULL));
		uint16_t L_8 = ___0_value;
		il2cpp_codegen_stind<int16_t>((int16_t*)((uint16_t*)il2cpp_codegen_add((intptr_t)L_5, ((intptr_t)il2cpp_codegen_multiply(L_7, 2)))), (int16_t)L_8);
		int32_t L_9 = V_8;
		V_8 = ((int32_t)il2cpp_codegen_add(L_9, 1));
	}

IL_00ff:
	{
		int32_t L_10 = V_8;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_11;
		L_11 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_10) < ((int32_t)L_11)))
		{
			goto IL_00e5;
		}
	}
	{
		uintptr_t L_12 = (il2cpp_codegen_conv<uintptr_t,int32_t,int32_t,false,false>(0,NULL));
		V_7 = (uint16_t*)L_12;
		return;
	}

IL_0386:
	{
		goto IL_0505;
	}

IL_0505:
	{
		goto IL_0684;
	}

IL_0684:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_13 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_14 = ___0_value;
		L_13->___uint16_0 = L_14;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_15 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_16 = ___0_value;
		L_15->___uint16_1 = L_16;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_17 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_18 = ___0_value;
		L_17->___uint16_2 = L_18;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_19 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_20 = ___0_value;
		L_19->___uint16_3 = L_20;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_21 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_22 = ___0_value;
		L_21->___uint16_4 = L_22;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_23 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_24 = ___0_value;
		L_23->___uint16_5 = L_24;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_25 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_26 = ___0_value;
		L_25->___uint16_6 = L_26;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_27 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_28 = ___0_value;
		L_27->___uint16_7 = L_28;
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427_AdjustorThunk (RuntimeObject* __this, uint16_t ___0_value, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427(_thisAdjusted, ___0_value, method);
}
// Method Definition Index: 7875
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, void* ___0_dataPointer, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		void* L_0 = ___0_dataPointer;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359(__this, L_0, 0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 6));
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3_AdjustorThunk (RuntimeObject* __this, void* ___0_dataPointer, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3(_thisAdjusted, ___0_dataPointer, method);
}
// Method Definition Index: 7876
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	uint8_t* V_1 = NULL;
	uint8_t* V_2 = NULL;
	int32_t V_3 = 0;
	int8_t* V_4 = NULL;
	int8_t* V_5 = NULL;
	int8_t* V_6 = NULL;
	int32_t V_7 = 0;
	uint16_t* V_8 = NULL;
	uint16_t* V_9 = NULL;
	uint16_t* V_10 = NULL;
	int32_t V_11 = 0;
	int16_t* V_12 = NULL;
	int16_t* V_13 = NULL;
	int16_t* V_14 = NULL;
	int32_t V_15 = 0;
	uint32_t* V_16 = NULL;
	uint32_t* V_17 = NULL;
	uint32_t* V_18 = NULL;
	int32_t V_19 = 0;
	int32_t* V_20 = NULL;
	int32_t* V_21 = NULL;
	int32_t* V_22 = NULL;
	int32_t V_23 = 0;
	uint64_t* V_24 = NULL;
	uint64_t* V_25 = NULL;
	uint64_t* V_26 = NULL;
	int32_t V_27 = 0;
	int64_t* V_28 = NULL;
	int64_t* V_29 = NULL;
	int64_t* V_30 = NULL;
	int32_t V_31 = 0;
	float* V_32 = NULL;
	float* V_33 = NULL;
	float* V_34 = NULL;
	int32_t V_35 = 0;
	double* V_36 = NULL;
	double* V_37 = NULL;
	double* V_38 = NULL;
	int32_t V_39 = 0;
	{
		il2cpp_codegen_initobj(__this, sizeof(Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489));
		goto IL_0053;
	}

IL_0053:
	{
		goto IL_00ae;
	}

IL_00ae:
	{
	}
	{
		void* L_0 = ___0_dataPointer;
		V_8 = (uint16_t*)L_0;
		uint16_t* L_1 = V_8;
		int32_t L_2 = ___1_offset;
		intptr_t L_3 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_2,NULL));
		V_8 = ((uint16_t*)il2cpp_codegen_add((intptr_t)L_1, ((intptr_t)il2cpp_codegen_multiply(L_3, 2))));
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_4 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_5 = (uint16_t*)(&L_4->___uint16_0);
		V_10 = L_5;
		uint16_t* L_6 = V_10;
		uintptr_t L_7 = (il2cpp_codegen_conv<uintptr_t,uint16_t*,intptr_t,false,false>(L_6,NULL));
		V_9 = (uint16_t*)L_7;
		V_11 = 0;
		goto IL_0104;
	}

IL_00ec:
	{
		uint16_t* L_8 = V_9;
		int32_t L_9 = V_11;
		intptr_t L_10 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_9,NULL));
		uint16_t* L_11 = V_8;
		int32_t L_12 = V_11;
		intptr_t L_13 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_12,NULL));
		int32_t L_14 = il2cpp_codegen_ldind<int32_t, uint16_t>(((uint16_t*)il2cpp_codegen_add((intptr_t)L_11, ((intptr_t)il2cpp_codegen_multiply(L_13, 2)))));
		il2cpp_codegen_stind<int16_t>((int16_t*)((uint16_t*)il2cpp_codegen_add((intptr_t)L_8, ((intptr_t)il2cpp_codegen_multiply(L_10, 2)))), (int16_t)L_14);
		int32_t L_15 = V_11;
		V_11 = ((int32_t)il2cpp_codegen_add(L_15, 1));
	}

IL_0104:
	{
		int32_t L_16 = V_11;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_17;
		L_17 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_16) < ((int32_t)L_17)))
		{
			goto IL_00ec;
		}
	}
	{
		uintptr_t L_18 = (il2cpp_codegen_conv<uintptr_t,int32_t,int32_t,false,false>(0,NULL));
		V_10 = (uint16_t*)L_18;
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359_AdjustorThunk (RuntimeObject* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	Vector_1__ctor_m8212BCFF76673CC904541B2D9AF39E5FF124B359(_thisAdjusted, ___0_dataPointer, ___1_offset, method);
}
// Method Definition Index: 7877
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_0 = ___0_existingRegister;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_1 = (*(Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)L_0);
		__this->___register = L_1;
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A_AdjustorThunk (RuntimeObject* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A(_thisAdjusted, ___0_existingRegister, method);
}
// Method Definition Index: 7878
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint16_t Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, int32_t ___0_index, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	int8_t* V_1 = NULL;
	uint16_t* V_2 = NULL;
	int16_t* V_3 = NULL;
	uint32_t* V_4 = NULL;
	int32_t* V_5 = NULL;
	uint64_t* V_6 = NULL;
	int64_t* V_7 = NULL;
	float* V_8 = NULL;
	double* V_9 = NULL;
	{
		int32_t L_0 = ___0_index;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_1;
		L_1 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_0) >= ((int32_t)L_1)))
		{
			goto IL_000c;
		}
	}
	{
		int32_t L_2 = ___0_index;
		if ((((int32_t)L_2) >= ((int32_t)0)))
		{
			goto IL_0022;
		}
	}

IL_000c:
	{
		int32_t L_3 = ___0_index;
		int32_t L_4 = L_3;
		RuntimeObject* L_5 = Box(il2cpp_defaults.int32_class, &L_4);
		String_t* L_6;
		L_6 = SR_Format_m9E8DC9AEFDC34AC67473EFAEAB78C5066C1A0D09(((String_t*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&_stringLiteral491788442E76F5D7830F0DBFCF8EDD98854F636F)), L_5, NULL);
		IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82* L_7 = (IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82*)il2cpp_codegen_object_new(((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82_il2cpp_TypeInfo_var)));
		IndexOutOfRangeException__ctor_mFD06819F05B815BE2D6E826D4E04F4C449D0A425(L_7, L_6, NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, method);
	}

IL_0022:
	{
		goto IL_0059;
	}

IL_0059:
	{
		goto IL_0090;
	}

IL_0090:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_8 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_9 = (uint16_t*)(&L_8->___uint16_0);
		V_2 = L_9;
		uint16_t* L_10 = V_2;
		uintptr_t L_11 = (il2cpp_codegen_conv<uintptr_t,uint16_t*,intptr_t,false,false>(L_10,NULL));
		int32_t L_12 = ___0_index;
		intptr_t L_13 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_12,NULL));
		int32_t L_14 = il2cpp_codegen_ldind<int32_t, uint16_t>(((uint16_t*)((intptr_t)il2cpp_codegen_add((intptr_t)L_11, ((intptr_t)il2cpp_codegen_multiply(L_13, 2))))));
		return (uint16_t)L_14;
	}
}
IL2CPP_EXTERN_C  uint16_t Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	uint16_t _returnValue;
	_returnValue = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27(_thisAdjusted, ___0_index, method);
	return _returnValue;
}
// Method Definition Index: 7879
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		RuntimeObject* L_0 = ___0_obj;
		if (((RuntimeObject*)IsInstSealed((RuntimeObject*)L_0, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0))))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject* L_1 = ___0_obj;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_2;
		L_2 = Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8(__this, ((*(Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*)UnBox(L_1, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0)))), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_AdjustorThunk (RuntimeObject* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	bool _returnValue;
	_returnValue = Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_inline(_thisAdjusted, ___0_obj, method);
	return _returnValue;
}
// Method Definition Index: 7880
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_other, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	{
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_0031;
		}
	}
	{
		V_0 = 0;
		goto IL_0027;
	}

IL_000b:
	{
		int32_t L_1 = V_0;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint16_t L_2;
		L_2 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27(__this, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		int32_t L_3 = V_0;
		uint16_t L_4;
		L_4 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27((&___0_other), L_3, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		bool L_5;
		L_5 = Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_inline(L_2, L_4, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 9));
		if (L_5)
		{
			goto IL_0023;
		}
	}
	{
		return (bool)0;
	}

IL_0023:
	{
		int32_t L_6 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_add(L_6, 1));
	}

IL_0027:
	{
		int32_t L_7 = V_0;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_8;
		L_8 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_7) < ((int32_t)L_8)))
		{
			goto IL_000b;
		}
	}
	{
		return (bool)1;
	}

IL_0031:
	{
		goto IL_01f0;
	}

IL_01f0:
	{
		goto IL_03af;
	}

IL_03af:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_9 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_10 = L_9->___uint16_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_11 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_12 = L_11.___register;
		uint16_t L_13 = L_12.___uint16_0;
		if ((!(((uint32_t)L_10) == ((uint32_t)L_13))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_14 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_15 = L_14->___uint16_1;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_16 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_17 = L_16.___register;
		uint16_t L_18 = L_17.___uint16_1;
		if ((!(((uint32_t)L_15) == ((uint32_t)L_18))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_19 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_20 = L_19->___uint16_2;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_21 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_22 = L_21.___register;
		uint16_t L_23 = L_22.___uint16_2;
		if ((!(((uint32_t)L_20) == ((uint32_t)L_23))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_24 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_25 = L_24->___uint16_3;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_26 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_27 = L_26.___register;
		uint16_t L_28 = L_27.___uint16_3;
		if ((!(((uint32_t)L_25) == ((uint32_t)L_28))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_29 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_30 = L_29->___uint16_4;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_31 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_32 = L_31.___register;
		uint16_t L_33 = L_32.___uint16_4;
		if ((!(((uint32_t)L_30) == ((uint32_t)L_33))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_34 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_35 = L_34->___uint16_5;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_36 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_37 = L_36.___register;
		uint16_t L_38 = L_37.___uint16_5;
		if ((!(((uint32_t)L_35) == ((uint32_t)L_38))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_39 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_40 = L_39->___uint16_6;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_41 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_42 = L_41.___register;
		uint16_t L_43 = L_42.___uint16_6;
		if ((!(((uint32_t)L_40) == ((uint32_t)L_43))))
		{
			goto IL_0494;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_44 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t L_45 = L_44->___uint16_7;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_46 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_47 = L_46.___register;
		uint16_t L_48 = L_47.___uint16_7;
		return (bool)((((int32_t)L_45) == ((int32_t)L_48))? 1 : 0);
	}

IL_0494:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8_AdjustorThunk (RuntimeObject* __this, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_other, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	bool _returnValue;
	_returnValue = Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8(_thisAdjusted, ___0_other, method);
	return _returnValue;
}
// Method Definition Index: 7881
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_GetHashCode_m3C7CFE908C6BB2DC94F94F7615F2D1AF0E2777D9_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	uint8_t V_2 = 0x0;
	int32_t V_3 = 0;
	int8_t V_4 = 0x0;
	int32_t V_5 = 0;
	uint16_t V_6 = 0;
	int32_t V_7 = 0;
	int16_t V_8 = 0;
	int32_t V_9 = 0;
	uint32_t V_10 = 0;
	int32_t V_11 = 0;
	int32_t V_12 = 0;
	int32_t V_13 = 0;
	uint64_t V_14 = 0;
	int32_t V_15 = 0;
	int64_t V_16 = 0;
	int32_t V_17 = 0;
	float V_18 = 0.0f;
	int32_t V_19 = 0;
	double V_20 = 0.0;
	{
		V_0 = 0;
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_034a;
		}
	}
	{
		goto IL_0059;
	}

IL_0059:
	{
		goto IL_00a7;
	}

IL_00a7:
	{
	}
	{
		V_5 = 0;
		goto IL_00ef;
	}

IL_00c7:
	{
		int32_t L_1 = V_0;
		int32_t L_2 = V_5;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint16_t L_3;
		L_3 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27(__this, L_2, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		V_6 = L_3;
		int32_t L_4;
		L_4 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200((&V_6), NULL);
		il2cpp_codegen_runtime_class_init_inline(HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var);
		int32_t L_5;
		L_5 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_1, L_4, NULL);
		V_0 = L_5;
		int32_t L_6 = V_5;
		V_5 = ((int32_t)il2cpp_codegen_add(L_6, 1));
	}

IL_00ef:
	{
		int32_t L_7 = V_5;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_8;
		L_8 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_7) < ((int32_t)L_8)))
		{
			goto IL_00c7;
		}
	}
	{
		int32_t L_9 = V_0;
		return L_9;
	}

IL_034a:
	{
		goto IL_04da;
	}

IL_04da:
	{
		goto IL_066a;
	}

IL_066a:
	{
	}
	{
		int32_t L_10 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_11 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_12 = (uint16_t*)(&L_11->___uint16_0);
		int32_t L_13;
		L_13 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_12, NULL);
		il2cpp_codegen_runtime_class_init_inline(HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var);
		int32_t L_14;
		L_14 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_10, L_13, NULL);
		V_0 = L_14;
		int32_t L_15 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_16 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_17 = (uint16_t*)(&L_16->___uint16_1);
		int32_t L_18;
		L_18 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_17, NULL);
		int32_t L_19;
		L_19 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_15, L_18, NULL);
		V_0 = L_19;
		int32_t L_20 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_21 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_22 = (uint16_t*)(&L_21->___uint16_2);
		int32_t L_23;
		L_23 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_22, NULL);
		int32_t L_24;
		L_24 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_20, L_23, NULL);
		V_0 = L_24;
		int32_t L_25 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_26 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_27 = (uint16_t*)(&L_26->___uint16_3);
		int32_t L_28;
		L_28 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_27, NULL);
		int32_t L_29;
		L_29 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_25, L_28, NULL);
		V_0 = L_29;
		int32_t L_30 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_31 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_32 = (uint16_t*)(&L_31->___uint16_4);
		int32_t L_33;
		L_33 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_32, NULL);
		int32_t L_34;
		L_34 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_30, L_33, NULL);
		V_0 = L_34;
		int32_t L_35 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_36 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_37 = (uint16_t*)(&L_36->___uint16_5);
		int32_t L_38;
		L_38 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_37, NULL);
		int32_t L_39;
		L_39 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_35, L_38, NULL);
		V_0 = L_39;
		int32_t L_40 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_41 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_42 = (uint16_t*)(&L_41->___uint16_6);
		int32_t L_43;
		L_43 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_42, NULL);
		int32_t L_44;
		L_44 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_40, L_43, NULL);
		V_0 = L_44;
		int32_t L_45 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_46 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint16_t* L_47 = (uint16_t*)(&L_46->___uint16_7);
		int32_t L_48;
		L_48 = UInt16_GetHashCode_m534E5103D0DA9C6FCED4F2F007993D3E38165200(L_47, NULL);
		int32_t L_49;
		L_49 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_45, L_48, NULL);
		V_0 = L_49;
		int32_t L_50 = V_0;
		return L_50;
	}
}
IL2CPP_EXTERN_C  int32_t Vector_1_GetHashCode_m3C7CFE908C6BB2DC94F94F7615F2D1AF0E2777D9_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	int32_t _returnValue;
	_returnValue = Vector_1_GetHashCode_m3C7CFE908C6BB2DC94F94F7615F2D1AF0E2777D9(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 7882
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_m2444D8FDCF0568D259DAE989EB7BCC77D37B2D6D_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3DCC6243286938BE75C3FA773B9BA71160A2E869);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* L_0;
		L_0 = CultureInfo_get_CurrentCulture_m8A4580F49DDD7E9DB34C699965423DB8E3BBA9A5(NULL);
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		String_t* L_1;
		L_1 = Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7(__this, _stringLiteral3DCC6243286938BE75C3FA773B9BA71160A2E869, (RuntimeObject*)L_0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 10));
		return L_1;
	}
}
IL2CPP_EXTERN_C  String_t* Vector_1_ToString_m2444D8FDCF0568D259DAE989EB7BCC77D37B2D6D_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	String_t* _returnValue;
	_returnValue = Vector_1_ToString_m2444D8FDCF0568D259DAE989EB7BCC77D37B2D6D(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 7883
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&StringBuilder_t_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	StringBuilder_t* V_0 = NULL;
	String_t* V_1 = NULL;
	int32_t V_2 = 0;
	{
		StringBuilder_t* L_0 = (StringBuilder_t*)il2cpp_codegen_object_new(StringBuilder_t_il2cpp_TypeInfo_var);
		StringBuilder__ctor_m1D99713357DE05DAFA296633639DB55F8C30587D(L_0, NULL);
		V_0 = L_0;
		RuntimeObject* L_1 = ___1_formatProvider;
		NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* L_2;
		L_2 = NumberFormatInfo_GetInstance_m705987E5E7D3E5EC5C5DD2D088FBC9BCBA0FC31F(L_1, NULL);
		NullCheck(L_2);
		String_t* L_3;
		L_3 = NumberFormatInfo_get_NumberGroupSeparator_m0556B092AA471513B1EDC31C047712226D39BEB6_inline(L_2, NULL);
		V_1 = L_3;
		StringBuilder_t* L_4 = V_0;
		NullCheck(L_4);
		StringBuilder_t* L_5;
		L_5 = StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1(L_4, (Il2CppChar)((int32_t)60), NULL);
		V_2 = 0;
		goto IL_0053;
	}

IL_001f:
	{
		StringBuilder_t* L_6 = V_0;
		int32_t L_7 = V_2;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint16_t L_8;
		L_8 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27(__this, L_7, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		uint16_t L_9 = L_8;
		RuntimeObject* L_10 = Box(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4), &L_9);
		String_t* L_11 = ___0_format;
		RuntimeObject* L_12 = ___1_formatProvider;
		String_t* L_13;
		L_13 = UInt16_ToString_mBD648884B6569D3E7D779669EEFCB1ED5EE4A521((uint16_t*)UnBox(L_10, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 4)), L_11, L_12, NULL);
		NullCheck(L_6);
		StringBuilder_t* L_14;
		L_14 = StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D(L_6, L_13, NULL);
		StringBuilder_t* L_15 = V_0;
		String_t* L_16 = V_1;
		NullCheck(L_15);
		StringBuilder_t* L_17;
		L_17 = StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D(L_15, L_16, NULL);
		StringBuilder_t* L_18 = V_0;
		NullCheck(L_18);
		StringBuilder_t* L_19;
		L_19 = StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1(L_18, (Il2CppChar)((int32_t)32), NULL);
		int32_t L_20 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_20, 1));
	}

IL_0053:
	{
		int32_t L_21 = V_2;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_22;
		L_22 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_21) < ((int32_t)((int32_t)il2cpp_codegen_subtract(L_22, 1)))))
		{
			goto IL_001f;
		}
	}
	{
		StringBuilder_t* L_23 = V_0;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_24;
		L_24 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		uint16_t L_25;
		L_25 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27(__this, ((int32_t)il2cpp_codegen_subtract(L_24, 1)), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		uint16_t L_26 = L_25;
		RuntimeObject* L_27 = Box(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4), &L_26);
		String_t* L_28 = ___0_format;
		RuntimeObject* L_29 = ___1_formatProvider;
		String_t* L_30;
		L_30 = UInt16_ToString_mBD648884B6569D3E7D779669EEFCB1ED5EE4A521((uint16_t*)UnBox(L_27, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 4)), L_28, L_29, NULL);
		NullCheck(L_23);
		StringBuilder_t* L_31;
		L_31 = StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D(L_23, L_30, NULL);
		StringBuilder_t* L_32 = V_0;
		NullCheck(L_32);
		StringBuilder_t* L_33;
		L_33 = StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1(L_32, (Il2CppChar)((int32_t)62), NULL);
		StringBuilder_t* L_34 = V_0;
		NullCheck((RuntimeObject*)L_34);
		String_t* L_35;
		L_35 = VirtualFuncInvoker0< String_t* >::Invoke(3, (RuntimeObject*)L_34);
		return L_35;
	}
}
IL2CPP_EXTERN_C  String_t* Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7_AdjustorThunk (RuntimeObject* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method)
{
	Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489>(__this);
	String_t* _returnValue;
	_returnValue = Vector_1_ToString_mA9FEB41834880EF7C7688EB8C3F83286697B0BC7(_thisAdjusted, ___0_format, ___1_formatProvider, method);
	return _returnValue;
}
// Method Definition Index: 7884
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_op_Equality_mB42F3DAE52C3BC7579B302E623196C45A5DEAC6B_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_left, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_0 = ___1_right;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_1;
		L_1 = Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8((&___0_left), L_0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_1;
	}
}
// Method Definition Index: 7885
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_op_Inequality_m32F4DFF513244591C00E23353EB6F6294E8BE9F0_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_left, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_0 = ___0_left;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_1 = ___1_right;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_2;
		L_2 = Vector_1_op_Equality_mB42F3DAE52C3BC7579B302E623196C45A5DEAC6B_inline(L_0, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 11));
		return (bool)((((int32_t)L_2) == ((int32_t)0))? 1 : 0);
	}
}
// Method Definition Index: 7886
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A Vector_1_op_Explicit_m5E44D3923BF92F437AEC34CDE0CBD6130883B0B7_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_value, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_0 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&(&___0_value)->___register);
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_1;
		memset((&L_1), 0, sizeof(L_1));
		Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606((&L_1), L_0, Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_RuntimeMethod_var);
		return L_1;
	}
}
// Method Definition Index: 7887
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 Vector_1_Equals_m6F913845CB1E8A1A753B3C187A8EB840C36F5ADF_gshared (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_left, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	int32_t V_1 = 0;
	int8_t* V_2 = NULL;
	int32_t V_3 = 0;
	uint16_t* V_4 = NULL;
	int32_t V_5 = 0;
	int16_t* V_6 = NULL;
	int32_t V_7 = 0;
	uint32_t* V_8 = NULL;
	int32_t V_9 = 0;
	int32_t* V_10 = NULL;
	int32_t V_11 = 0;
	uint64_t* V_12 = NULL;
	int32_t V_13 = 0;
	int64_t* V_14 = NULL;
	int32_t V_15 = 0;
	float* V_16 = NULL;
	int32_t V_17 = 0;
	double* V_18 = NULL;
	int32_t V_19 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A V_20;
	memset((&V_20), 0, sizeof(V_20));
	uint16_t* G_B21_0 = NULL;
	uint16_t* G_B20_0 = NULL;
	int32_t G_B22_0 = 0;
	uint16_t* G_B22_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B185_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B184_0 = NULL;
	int32_t G_B186_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B186_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B188_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B187_0 = NULL;
	int32_t G_B189_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B189_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B191_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B190_0 = NULL;
	int32_t G_B192_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B192_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B194_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B193_0 = NULL;
	int32_t G_B195_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B195_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B197_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B196_0 = NULL;
	int32_t G_B198_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B198_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B200_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B199_0 = NULL;
	int32_t G_B201_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B201_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B203_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B202_0 = NULL;
	int32_t G_B204_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B204_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B206_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B205_0 = NULL;
	int32_t G_B207_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B207_1 = NULL;
	{
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_0447;
		}
	}
	{
		goto IL_0068;
	}

IL_0068:
	{
		goto IL_00c6;
	}

IL_00c6:
	{
	}
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_1;
		L_1 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		uintptr_t L_2 = (il2cpp_codegen_conv<uintptr_t,int32_t,int32_t,false,false>(L_1,NULL));
		if ((uintptr_t)L_2 * (uintptr_t)2 > (uintptr_t)kIl2CppUIntPtrMax)
			IL2CPP_RAISE_MANAGED_EXCEPTION(il2cpp_codegen_get_overflow_exception(), method);
		intptr_t L_3 = ((intptr_t)il2cpp_codegen_multiply((intptr_t)L_2, 2));
		int8_t* L_4;
		if (L_3 == 0)
		{
			L_4 = NULL;
		}
		else
		{
			L_4 = (int8_t*)alloca(L_3);
			memset(L_4, 0, L_3);
		}
		V_4 = (uint16_t*)L_4;
		V_5 = 0;
		goto IL_0122;
	}

IL_00f2:
	{
		uint16_t* L_5 = V_4;
		int32_t L_6 = V_5;
		intptr_t L_7 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_6,NULL));
		int32_t L_8 = V_5;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint16_t L_9;
		L_9 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27((&___0_left), L_8, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		int32_t L_10 = V_5;
		uint16_t L_11;
		L_11 = Vector_1_get_Item_m248FFF521980A3A43D237B8C5CA2ABD5C62D4D27((&___1_right), L_10, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		bool L_12;
		L_12 = Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_inline(L_9, L_11, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 9));
		if (L_12)
		{
			G_B21_0 = ((uint16_t*)il2cpp_codegen_add((intptr_t)L_5, ((intptr_t)il2cpp_codegen_multiply(L_7, 2))));
			goto IL_0116;
		}
		G_B20_0 = ((uint16_t*)il2cpp_codegen_add((intptr_t)L_5, ((intptr_t)il2cpp_codegen_multiply(L_7, 2))));
	}
	{
		G_B22_0 = 0;
		G_B22_1 = G_B20_0;
		goto IL_011b;
	}

IL_0116:
	{
		uint16_t L_13;
		L_13 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B22_0 = ((int32_t)(L_13));
		G_B22_1 = G_B21_0;
	}

IL_011b:
	{
		il2cpp_codegen_stind<int16_t>((int16_t*)G_B22_1, (int16_t)G_B22_0);
		int32_t L_14 = V_5;
		V_5 = ((int32_t)il2cpp_codegen_add(L_14, 1));
	}

IL_0122:
	{
		int32_t L_15 = V_5;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_16;
		L_16 = Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_15) < ((int32_t)L_16)))
		{
			goto IL_00f2;
		}
	}
	{
		uint16_t* L_17 = V_4;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_18;
		memset((&L_18), 0, sizeof(L_18));
		Vector_1__ctor_m46ADEA122EFBA7AEF487716891A8ADD284FD12E3((&L_18), (void*)L_17, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 12));
		return L_18;
	}

IL_0447:
	{
		il2cpp_codegen_initobj((&V_20), sizeof(Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A));
		goto IL_06e5;
	}

IL_06e5:
	{
		goto IL_097b;
	}

IL_097b:
	{
	}
	{
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_19 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_20 = L_19.___register;
		uint16_t L_21 = L_20.___uint16_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_22 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_23 = L_22.___register;
		uint16_t L_24 = L_23.___uint16_0;
		if ((((int32_t)L_21) == ((int32_t)L_24)))
		{
			G_B185_0 = (&V_20);
			goto IL_09b6;
		}
		G_B184_0 = (&V_20);
	}
	{
		G_B186_0 = 0;
		G_B186_1 = G_B184_0;
		goto IL_09bb;
	}

IL_09b6:
	{
		uint16_t L_25;
		L_25 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B186_0 = ((int32_t)(L_25));
		G_B186_1 = G_B185_0;
	}

IL_09bb:
	{
		G_B186_1->___uint16_0 = (uint16_t)G_B186_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_26 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_27 = L_26.___register;
		uint16_t L_28 = L_27.___uint16_1;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_29 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_30 = L_29.___register;
		uint16_t L_31 = L_30.___uint16_1;
		if ((((int32_t)L_28) == ((int32_t)L_31)))
		{
			G_B188_0 = (&V_20);
			goto IL_09dd;
		}
		G_B187_0 = (&V_20);
	}
	{
		G_B189_0 = 0;
		G_B189_1 = G_B187_0;
		goto IL_09e2;
	}

IL_09dd:
	{
		uint16_t L_32;
		L_32 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B189_0 = ((int32_t)(L_32));
		G_B189_1 = G_B188_0;
	}

IL_09e2:
	{
		G_B189_1->___uint16_1 = (uint16_t)G_B189_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_33 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_34 = L_33.___register;
		uint16_t L_35 = L_34.___uint16_2;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_36 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_37 = L_36.___register;
		uint16_t L_38 = L_37.___uint16_2;
		if ((((int32_t)L_35) == ((int32_t)L_38)))
		{
			G_B191_0 = (&V_20);
			goto IL_0a04;
		}
		G_B190_0 = (&V_20);
	}
	{
		G_B192_0 = 0;
		G_B192_1 = G_B190_0;
		goto IL_0a09;
	}

IL_0a04:
	{
		uint16_t L_39;
		L_39 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B192_0 = ((int32_t)(L_39));
		G_B192_1 = G_B191_0;
	}

IL_0a09:
	{
		G_B192_1->___uint16_2 = (uint16_t)G_B192_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_40 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_41 = L_40.___register;
		uint16_t L_42 = L_41.___uint16_3;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_43 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_44 = L_43.___register;
		uint16_t L_45 = L_44.___uint16_3;
		if ((((int32_t)L_42) == ((int32_t)L_45)))
		{
			G_B194_0 = (&V_20);
			goto IL_0a2b;
		}
		G_B193_0 = (&V_20);
	}
	{
		G_B195_0 = 0;
		G_B195_1 = G_B193_0;
		goto IL_0a30;
	}

IL_0a2b:
	{
		uint16_t L_46;
		L_46 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B195_0 = ((int32_t)(L_46));
		G_B195_1 = G_B194_0;
	}

IL_0a30:
	{
		G_B195_1->___uint16_3 = (uint16_t)G_B195_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_47 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_48 = L_47.___register;
		uint16_t L_49 = L_48.___uint16_4;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_50 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_51 = L_50.___register;
		uint16_t L_52 = L_51.___uint16_4;
		if ((((int32_t)L_49) == ((int32_t)L_52)))
		{
			G_B197_0 = (&V_20);
			goto IL_0a52;
		}
		G_B196_0 = (&V_20);
	}
	{
		G_B198_0 = 0;
		G_B198_1 = G_B196_0;
		goto IL_0a57;
	}

IL_0a52:
	{
		uint16_t L_53;
		L_53 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B198_0 = ((int32_t)(L_53));
		G_B198_1 = G_B197_0;
	}

IL_0a57:
	{
		G_B198_1->___uint16_4 = (uint16_t)G_B198_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_54 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_55 = L_54.___register;
		uint16_t L_56 = L_55.___uint16_5;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_57 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_58 = L_57.___register;
		uint16_t L_59 = L_58.___uint16_5;
		if ((((int32_t)L_56) == ((int32_t)L_59)))
		{
			G_B200_0 = (&V_20);
			goto IL_0a79;
		}
		G_B199_0 = (&V_20);
	}
	{
		G_B201_0 = 0;
		G_B201_1 = G_B199_0;
		goto IL_0a7e;
	}

IL_0a79:
	{
		uint16_t L_60;
		L_60 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B201_0 = ((int32_t)(L_60));
		G_B201_1 = G_B200_0;
	}

IL_0a7e:
	{
		G_B201_1->___uint16_5 = (uint16_t)G_B201_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_61 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_62 = L_61.___register;
		uint16_t L_63 = L_62.___uint16_6;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_64 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_65 = L_64.___register;
		uint16_t L_66 = L_65.___uint16_6;
		if ((((int32_t)L_63) == ((int32_t)L_66)))
		{
			G_B203_0 = (&V_20);
			goto IL_0aa0;
		}
		G_B202_0 = (&V_20);
	}
	{
		G_B204_0 = 0;
		G_B204_1 = G_B202_0;
		goto IL_0aa5;
	}

IL_0aa0:
	{
		uint16_t L_67;
		L_67 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B204_0 = ((int32_t)(L_67));
		G_B204_1 = G_B203_0;
	}

IL_0aa5:
	{
		G_B204_1->___uint16_6 = (uint16_t)G_B204_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_68 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_69 = L_68.___register;
		uint16_t L_70 = L_69.___uint16_7;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_71 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_72 = L_71.___register;
		uint16_t L_73 = L_72.___uint16_7;
		if ((((int32_t)L_70) == ((int32_t)L_73)))
		{
			G_B206_0 = (&V_20);
			goto IL_0ac7;
		}
		G_B205_0 = (&V_20);
	}
	{
		G_B207_0 = 0;
		G_B207_1 = G_B205_0;
		goto IL_0acc;
	}

IL_0ac7:
	{
		uint16_t L_74;
		L_74 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		G_B207_0 = ((int32_t)(L_74));
		G_B207_1 = G_B206_0;
	}

IL_0acc:
	{
		G_B207_1->___uint16_7 = (uint16_t)G_B207_0;
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_75;
		memset((&L_75), 0, sizeof(L_75));
		Vector_1__ctor_m48CD7847B9597F3193C9C0BA97ED64E276F4340A((&L_75), (&V_20), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 13));
		return L_75;
	}
}
// Method Definition Index: 7888
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_gshared (uint16_t ___0_left, uint16_t ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0034;
	}

IL_0034:
	{
		goto IL_0068;
	}

IL_0068:
	{
	}
	{
		uint16_t L_0 = ___0_left;
		uint16_t L_1 = ___1_right;
		return (bool)((((int32_t)L_0) == ((int32_t)L_1))? 1 : 0);
	}
}
// Method Definition Index: 7889
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint16_t Vector_1_GetOneValue_m7E814AFD17E4D390C12EF731DA01203D262D9953_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0027;
	}

IL_0027:
	{
		goto IL_004e;
	}

IL_004e:
	{
	}
	{
		return (uint16_t)1;
	}
}
// Method Definition Index: 7890
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint16_t Vector_1_GetAllBitsSetValue_m854DE079EA89F97089D3EF29D7C31F081F420580_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_002b;
	}

IL_002b:
	{
		goto IL_0056;
	}

IL_0056:
	{
	}
	{
		uint16_t L_0;
		L_0 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		return L_0;
	}
}
// Method Definition Index: 7891
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__cctor_m6764AA686DE7E188C362C5B3E96AB2F5AB09F3CA_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		int32_t L_0;
		L_0 = Vector_1_InitializeCount_m43BBDDA05FDAB290038584331DB79CB33C523B83(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 14));
		((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_count = L_0;
		il2cpp_codegen_initobj((&((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_zero), sizeof(Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489));
		uint16_t L_1;
		L_1 = Vector_1_GetOneValue_m7E814AFD17E4D390C12EF731DA01203D262D9953_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 15));
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_2;
		memset((&L_2), 0, sizeof(L_2));
		Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427((&L_2), L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 16));
		((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_one = L_2;
		uint16_t L_3;
		L_3 = Vector_1_GetAllBitsSetValue_m854DE079EA89F97089D3EF29D7C31F081F420580_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 17));
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_4;
		memset((&L_4), 0, sizeof(L_4));
		Vector_1__ctor_mDA4A6115C4120BFDD773FD4D3753FD3EC2B10427((&L_4), L_3, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 16));
		((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_allOnes = L_4;
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 7871
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_0 = ((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_count;
		return L_0;
	}
}
// Method Definition Index: 7872
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A Vector_1_get_Zero_m052680C155E15387C16A5E044176ACB59DF53359_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_0 = ((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_zero;
		return L_0;
	}
}
// Method Definition Index: 7873
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_InitializeCount_mE29E088973A17B81B830C30831075135FC8E263A_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	VectorSizeHelper_tF54ACCE947CB8A38047BEB642392A4E7345A157D V_0;
	memset((&V_0), 0, sizeof(V_0));
	uint8_t* V_1 = NULL;
	int32_t V_2 = 0;
	int32_t G_B2_0 = 0;
	int32_t G_B21_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B6_0 = 0;
	int32_t G_B8_0 = 0;
	int32_t G_B10_0 = 0;
	int32_t G_B12_0 = 0;
	int32_t G_B13_0 = 0;
	{
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* L_0 = (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*)(&(&V_0)->____placeholder);
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_1 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&L_0->___register);
		uint8_t* L_2 = (uint8_t*)(&L_1->___byte_0);
		uintptr_t L_3 = (il2cpp_codegen_conv<uintptr_t,uint8_t*,intptr_t,false,false>(L_2,NULL));
		V_1 = (uint8_t*)L_3;
		uint8_t* L_4 = (uint8_t*)(&(&V_0)->____byte);
		uintptr_t L_5 = (il2cpp_codegen_conv<uintptr_t,uint8_t*,intptr_t,false,false>(L_4,NULL));
		uint8_t* L_6 = V_1;
		int64_t L_7 = (il2cpp_codegen_conv<int64_t,uint8_t*,intptr_t,false,false>(((uint8_t*)((intptr_t)((uint8_t*)il2cpp_codegen_subtract((intptr_t)L_5, (intptr_t)L_6))/1)),NULL));
		int32_t L_8 = (il2cpp_codegen_conv<int32_t,int64_t,int64_t,false,false>(L_7,NULL));
		V_2 = (-1);
		G_B2_0 = L_8;
		goto IL_0045;
	}

IL_0045:
	{
		G_B4_0 = G_B2_0;
		goto IL_0067;
	}

IL_0067:
	{
		G_B6_0 = G_B4_0;
		goto IL_0089;
	}

IL_0089:
	{
		G_B8_0 = G_B6_0;
		goto IL_00ab;
	}

IL_00ab:
	{
		G_B10_0 = G_B8_0;
		goto IL_00cd;
	}

IL_00cd:
	{
		G_B12_0 = G_B10_0;
		goto IL_00ef;
	}

IL_00ef:
	{
		G_B13_0 = G_B12_0;
	}
	{
		V_2 = 8;
		G_B21_0 = G_B13_0;
		goto IL_0176;
	}

IL_0176:
	{
		int32_t L_9 = V_2;
		return ((int32_t)(G_B21_0/L_9));
	}
}
// Method Definition Index: 7874
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, uint64_t ___0_value, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	uint8_t* V_1 = NULL;
	int32_t V_2 = 0;
	int8_t* V_3 = NULL;
	int8_t* V_4 = NULL;
	int32_t V_5 = 0;
	uint16_t* V_6 = NULL;
	uint16_t* V_7 = NULL;
	int32_t V_8 = 0;
	int16_t* V_9 = NULL;
	int16_t* V_10 = NULL;
	int32_t V_11 = 0;
	uint32_t* V_12 = NULL;
	uint32_t* V_13 = NULL;
	int32_t V_14 = 0;
	int32_t* V_15 = NULL;
	int32_t* V_16 = NULL;
	int32_t V_17 = 0;
	uint64_t* V_18 = NULL;
	uint64_t* V_19 = NULL;
	int32_t V_20 = 0;
	int64_t* V_21 = NULL;
	int64_t* V_22 = NULL;
	int32_t V_23 = 0;
	float* V_24 = NULL;
	float* V_25 = NULL;
	int32_t V_26 = 0;
	double* V_27 = NULL;
	double* V_28 = NULL;
	int32_t V_29 = 0;
	{
		il2cpp_codegen_initobj(__this, sizeof(Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A));
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_0386;
		}
	}
	{
		goto IL_005e;
	}

IL_005e:
	{
		goto IL_00b3;
	}

IL_00b3:
	{
		goto IL_010d;
	}

IL_010d:
	{
		goto IL_0167;
	}

IL_0167:
	{
		goto IL_01c1;
	}

IL_01c1:
	{
		goto IL_021b;
	}

IL_021b:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_1 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t* L_2 = (uint64_t*)(&L_1->___uint64_0);
		V_19 = L_2;
		uint64_t* L_3 = V_19;
		uintptr_t L_4 = (il2cpp_codegen_conv<uintptr_t,uint64_t*,intptr_t,false,false>(L_3,NULL));
		V_18 = (uint64_t*)L_4;
		V_20 = 0;
		goto IL_0267;
	}

IL_024d:
	{
		uint64_t* L_5 = V_18;
		int32_t L_6 = V_20;
		intptr_t L_7 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_6,NULL));
		uint64_t L_8 = ___0_value;
		il2cpp_codegen_stind<int64_t>((int64_t*)((uint64_t*)il2cpp_codegen_add((intptr_t)L_5, ((intptr_t)il2cpp_codegen_multiply(L_7, 8)))), (int64_t)L_8);
		int32_t L_9 = V_20;
		V_20 = ((int32_t)il2cpp_codegen_add(L_9, 1));
	}

IL_0267:
	{
		int32_t L_10 = V_20;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_11;
		L_11 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_10) < ((int32_t)L_11)))
		{
			goto IL_024d;
		}
	}
	{
		uintptr_t L_12 = (il2cpp_codegen_conv<uintptr_t,int32_t,int32_t,false,false>(0,NULL));
		V_19 = (uint64_t*)L_12;
		return;
	}

IL_0386:
	{
		goto IL_0505;
	}

IL_0505:
	{
		goto IL_0684;
	}

IL_0684:
	{
		goto IL_0753;
	}

IL_0753:
	{
		goto IL_0822;
	}

IL_0822:
	{
		goto IL_0896;
	}

IL_0896:
	{
		goto IL_090a;
	}

IL_090a:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_13 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t L_14 = ___0_value;
		L_13->___uint64_0 = L_14;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_15 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t L_16 = ___0_value;
		L_15->___uint64_1 = L_16;
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6_AdjustorThunk (RuntimeObject* __this, uint64_t ___0_value, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6(_thisAdjusted, ___0_value, method);
}
// Method Definition Index: 7875
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, void* ___0_dataPointer, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		void* L_0 = ___0_dataPointer;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B(__this, L_0, 0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 6));
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA_AdjustorThunk (RuntimeObject* __this, void* ___0_dataPointer, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA(_thisAdjusted, ___0_dataPointer, method);
}
// Method Definition Index: 7876
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	uint8_t* V_1 = NULL;
	uint8_t* V_2 = NULL;
	int32_t V_3 = 0;
	int8_t* V_4 = NULL;
	int8_t* V_5 = NULL;
	int8_t* V_6 = NULL;
	int32_t V_7 = 0;
	uint16_t* V_8 = NULL;
	uint16_t* V_9 = NULL;
	uint16_t* V_10 = NULL;
	int32_t V_11 = 0;
	int16_t* V_12 = NULL;
	int16_t* V_13 = NULL;
	int16_t* V_14 = NULL;
	int32_t V_15 = 0;
	uint32_t* V_16 = NULL;
	uint32_t* V_17 = NULL;
	uint32_t* V_18 = NULL;
	int32_t V_19 = 0;
	int32_t* V_20 = NULL;
	int32_t* V_21 = NULL;
	int32_t* V_22 = NULL;
	int32_t V_23 = 0;
	uint64_t* V_24 = NULL;
	uint64_t* V_25 = NULL;
	uint64_t* V_26 = NULL;
	int32_t V_27 = 0;
	int64_t* V_28 = NULL;
	int64_t* V_29 = NULL;
	int64_t* V_30 = NULL;
	int32_t V_31 = 0;
	float* V_32 = NULL;
	float* V_33 = NULL;
	float* V_34 = NULL;
	int32_t V_35 = 0;
	double* V_36 = NULL;
	double* V_37 = NULL;
	double* V_38 = NULL;
	int32_t V_39 = 0;
	{
		il2cpp_codegen_initobj(__this, sizeof(Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A));
		goto IL_0053;
	}

IL_0053:
	{
		goto IL_00ae;
	}

IL_00ae:
	{
		goto IL_0112;
	}

IL_0112:
	{
		goto IL_0176;
	}

IL_0176:
	{
		goto IL_01da;
	}

IL_01da:
	{
		goto IL_023e;
	}

IL_023e:
	{
	}
	{
		void* L_0 = ___0_dataPointer;
		V_24 = (uint64_t*)L_0;
		uint64_t* L_1 = V_24;
		int32_t L_2 = ___1_offset;
		intptr_t L_3 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_2,NULL));
		V_24 = ((uint64_t*)il2cpp_codegen_add((intptr_t)L_1, ((intptr_t)il2cpp_codegen_multiply(L_3, 8))));
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_4 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t* L_5 = (uint64_t*)(&L_4->___uint64_0);
		V_26 = L_5;
		uint64_t* L_6 = V_26;
		uintptr_t L_7 = (il2cpp_codegen_conv<uintptr_t,uint64_t*,intptr_t,false,false>(L_6,NULL));
		V_25 = (uint64_t*)L_7;
		V_27 = 0;
		goto IL_0294;
	}

IL_027c:
	{
		uint64_t* L_8 = V_25;
		int32_t L_9 = V_27;
		intptr_t L_10 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_9,NULL));
		uint64_t* L_11 = V_24;
		int32_t L_12 = V_27;
		intptr_t L_13 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_12,NULL));
		int64_t L_14 = il2cpp_codegen_ldind<int64_t, int64_t>(((int64_t*)((uint64_t*)il2cpp_codegen_add((intptr_t)L_11, ((intptr_t)il2cpp_codegen_multiply(L_13, 8))))));
		il2cpp_codegen_stind<int64_t>((int64_t*)((uint64_t*)il2cpp_codegen_add((intptr_t)L_8, ((intptr_t)il2cpp_codegen_multiply(L_10, 8)))), (int64_t)L_14);
		int32_t L_15 = V_27;
		V_27 = ((int32_t)il2cpp_codegen_add(L_15, 1));
	}

IL_0294:
	{
		int32_t L_16 = V_27;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_17;
		L_17 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_16) < ((int32_t)L_17)))
		{
			goto IL_027c;
		}
	}
	{
		uintptr_t L_18 = (il2cpp_codegen_conv<uintptr_t,int32_t,int32_t,false,false>(0,NULL));
		V_26 = (uint64_t*)L_18;
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B_AdjustorThunk (RuntimeObject* __this, void* ___0_dataPointer, int32_t ___1_offset, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	Vector_1__ctor_mB3EB022FA5067096F41350560FA447FBA16BFF2B(_thisAdjusted, ___0_dataPointer, ___1_offset, method);
}
// Method Definition Index: 7877
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_0 = ___0_existingRegister;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_1 = (*(Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)L_0);
		__this->___register = L_1;
		return;
	}
}
IL2CPP_EXTERN_C  void Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_AdjustorThunk (RuntimeObject* __this, Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* ___0_existingRegister, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606(_thisAdjusted, ___0_existingRegister, method);
}
// Method Definition Index: 7878
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint64_t Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, int32_t ___0_index, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	int8_t* V_1 = NULL;
	uint16_t* V_2 = NULL;
	int16_t* V_3 = NULL;
	uint32_t* V_4 = NULL;
	int32_t* V_5 = NULL;
	uint64_t* V_6 = NULL;
	int64_t* V_7 = NULL;
	float* V_8 = NULL;
	double* V_9 = NULL;
	{
		int32_t L_0 = ___0_index;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_1;
		L_1 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_0) >= ((int32_t)L_1)))
		{
			goto IL_000c;
		}
	}
	{
		int32_t L_2 = ___0_index;
		if ((((int32_t)L_2) >= ((int32_t)0)))
		{
			goto IL_0022;
		}
	}

IL_000c:
	{
		int32_t L_3 = ___0_index;
		int32_t L_4 = L_3;
		RuntimeObject* L_5 = Box(il2cpp_defaults.int32_class, &L_4);
		String_t* L_6;
		L_6 = SR_Format_m9E8DC9AEFDC34AC67473EFAEAB78C5066C1A0D09(((String_t*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&_stringLiteral491788442E76F5D7830F0DBFCF8EDD98854F636F)), L_5, NULL);
		IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82* L_7 = (IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82*)il2cpp_codegen_object_new(((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&IndexOutOfRangeException_t7ECB35264FB6CA8FAA516BD958F4B2ADC78E8A82_il2cpp_TypeInfo_var)));
		IndexOutOfRangeException__ctor_mFD06819F05B815BE2D6E826D4E04F4C449D0A425(L_7, L_6, NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, method);
	}

IL_0022:
	{
		goto IL_0059;
	}

IL_0059:
	{
		goto IL_0090;
	}

IL_0090:
	{
		goto IL_00ca;
	}

IL_00ca:
	{
		goto IL_0104;
	}

IL_0104:
	{
		goto IL_0140;
	}

IL_0140:
	{
		goto IL_017c;
	}

IL_017c:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_8 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t* L_9 = (uint64_t*)(&L_8->___uint64_0);
		V_6 = L_9;
		uint64_t* L_10 = V_6;
		uintptr_t L_11 = (il2cpp_codegen_conv<uintptr_t,uint64_t*,intptr_t,false,false>(L_10,NULL));
		int32_t L_12 = ___0_index;
		intptr_t L_13 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_12,NULL));
		int64_t L_14 = il2cpp_codegen_ldind<int64_t, int64_t>(((int64_t*)((intptr_t)il2cpp_codegen_add((intptr_t)L_11, ((intptr_t)il2cpp_codegen_multiply(L_13, 8))))));
		return (uint64_t)L_14;
	}
}
IL2CPP_EXTERN_C  uint64_t Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	uint64_t _returnValue;
	_returnValue = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99(_thisAdjusted, ___0_index, method);
	return _returnValue;
}
// Method Definition Index: 7879
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		RuntimeObject* L_0 = ___0_obj;
		if (((RuntimeObject*)IsInstSealed((RuntimeObject*)L_0, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0))))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject* L_1 = ___0_obj;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_2;
		L_2 = Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27(__this, ((*(Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*)UnBox(L_1, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0)))), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_2;
	}
}
IL2CPP_EXTERN_C  bool Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_AdjustorThunk (RuntimeObject* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	bool _returnValue;
	_returnValue = Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_inline(_thisAdjusted, ___0_obj, method);
	return _returnValue;
}
// Method Definition Index: 7880
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_other, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	{
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_0031;
		}
	}
	{
		V_0 = 0;
		goto IL_0027;
	}

IL_000b:
	{
		int32_t L_1 = V_0;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint64_t L_2;
		L_2 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99(__this, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		int32_t L_3 = V_0;
		uint64_t L_4;
		L_4 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99((&___0_other), L_3, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		bool L_5;
		L_5 = Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_inline(L_2, L_4, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 9));
		if (L_5)
		{
			goto IL_0023;
		}
	}
	{
		return (bool)0;
	}

IL_0023:
	{
		int32_t L_6 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_add(L_6, 1));
	}

IL_0027:
	{
		int32_t L_7 = V_0;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_8;
		L_8 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_7) < ((int32_t)L_8)))
		{
			goto IL_000b;
		}
	}
	{
		return (bool)1;
	}

IL_0031:
	{
		goto IL_01f0;
	}

IL_01f0:
	{
		goto IL_03af;
	}

IL_03af:
	{
		goto IL_0496;
	}

IL_0496:
	{
		goto IL_057d;
	}

IL_057d:
	{
		goto IL_05fb;
	}

IL_05fb:
	{
		goto IL_0679;
	}

IL_0679:
	{
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_9 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t L_10 = L_9->___uint64_0;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_11 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_12 = L_11.___register;
		uint64_t L_13 = L_12.___uint64_0;
		if ((!(((uint64_t)L_10) == ((uint64_t)L_13))))
		{
			goto IL_06c5;
		}
	}
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_14 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t L_15 = L_14->___uint64_1;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_16 = ___0_other;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_17 = L_16.___register;
		uint64_t L_18 = L_17.___uint64_1;
		return (bool)((((int64_t)L_15) == ((int64_t)L_18))? 1 : 0);
	}

IL_06c5:
	{
		return (bool)0;
	}
}
IL2CPP_EXTERN_C  bool Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27_AdjustorThunk (RuntimeObject* __this, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_other, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	bool _returnValue;
	_returnValue = Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27(_thisAdjusted, ___0_other, method);
	return _returnValue;
}
// Method Definition Index: 7881
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Vector_1_GetHashCode_mEC951E56E2DC500CF877DFAD5542E0920B73B00A_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	uint8_t V_2 = 0x0;
	int32_t V_3 = 0;
	int8_t V_4 = 0x0;
	int32_t V_5 = 0;
	uint16_t V_6 = 0;
	int32_t V_7 = 0;
	int16_t V_8 = 0;
	int32_t V_9 = 0;
	uint32_t V_10 = 0;
	int32_t V_11 = 0;
	int32_t V_12 = 0;
	int32_t V_13 = 0;
	uint64_t V_14 = 0;
	int32_t V_15 = 0;
	int64_t V_16 = 0;
	int32_t V_17 = 0;
	float V_18 = 0.0f;
	int32_t V_19 = 0;
	double V_20 = 0.0;
	{
		V_0 = 0;
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_034a;
		}
	}
	{
		goto IL_0059;
	}

IL_0059:
	{
		goto IL_00a7;
	}

IL_00a7:
	{
		goto IL_00fa;
	}

IL_00fa:
	{
		goto IL_014d;
	}

IL_014d:
	{
		goto IL_01a0;
	}

IL_01a0:
	{
		goto IL_01f3;
	}

IL_01f3:
	{
	}
	{
		V_13 = 0;
		goto IL_023b;
	}

IL_0213:
	{
		int32_t L_1 = V_0;
		int32_t L_2 = V_13;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint64_t L_3;
		L_3 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99(__this, L_2, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		V_14 = L_3;
		int32_t L_4;
		L_4 = UInt64_GetHashCode_m65D9FD0102B6B01BF38D986F060F0BDBC29B4F92((&V_14), NULL);
		il2cpp_codegen_runtime_class_init_inline(HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var);
		int32_t L_5;
		L_5 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_1, L_4, NULL);
		V_0 = L_5;
		int32_t L_6 = V_13;
		V_13 = ((int32_t)il2cpp_codegen_add(L_6, 1));
	}

IL_023b:
	{
		int32_t L_7 = V_13;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_8;
		L_8 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_7) < ((int32_t)L_8)))
		{
			goto IL_0213;
		}
	}
	{
		int32_t L_9 = V_0;
		return L_9;
	}

IL_034a:
	{
		goto IL_04da;
	}

IL_04da:
	{
		goto IL_066a;
	}

IL_066a:
	{
		goto IL_0742;
	}

IL_0742:
	{
		goto IL_081a;
	}

IL_081a:
	{
		goto IL_0893;
	}

IL_0893:
	{
		goto IL_090c;
	}

IL_090c:
	{
	}
	{
		int32_t L_10 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_11 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t* L_12 = (uint64_t*)(&L_11->___uint64_0);
		int32_t L_13;
		L_13 = UInt64_GetHashCode_m65D9FD0102B6B01BF38D986F060F0BDBC29B4F92(L_12, NULL);
		il2cpp_codegen_runtime_class_init_inline(HashHelpers_t285C6E63B4A4E8D837BDBC63DE4E2D23C85467D4_il2cpp_TypeInfo_var);
		int32_t L_14;
		L_14 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_10, L_13, NULL);
		V_0 = L_14;
		int32_t L_15 = V_0;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_16 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&__this->___register);
		uint64_t* L_17 = (uint64_t*)(&L_16->___uint64_1);
		int32_t L_18;
		L_18 = UInt64_GetHashCode_m65D9FD0102B6B01BF38D986F060F0BDBC29B4F92(L_17, NULL);
		int32_t L_19;
		L_19 = HashHelpers_Combine_mBE398FF248FE6B082F5E254BCD36E3B3351608D7(L_15, L_18, NULL);
		V_0 = L_19;
		int32_t L_20 = V_0;
		return L_20;
	}
}
IL2CPP_EXTERN_C  int32_t Vector_1_GetHashCode_mEC951E56E2DC500CF877DFAD5542E0920B73B00A_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	int32_t _returnValue;
	_returnValue = Vector_1_GetHashCode_mEC951E56E2DC500CF877DFAD5542E0920B73B00A(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 7882
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_m3EBF88D3E195BD2C4B0D1CCBD9F71E32233CA4F4_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3DCC6243286938BE75C3FA773B9BA71160A2E869);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* L_0;
		L_0 = CultureInfo_get_CurrentCulture_m8A4580F49DDD7E9DB34C699965423DB8E3BBA9A5(NULL);
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		String_t* L_1;
		L_1 = Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C(__this, _stringLiteral3DCC6243286938BE75C3FA773B9BA71160A2E869, (RuntimeObject*)L_0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 10));
		return L_1;
	}
}
IL2CPP_EXTERN_C  String_t* Vector_1_ToString_m3EBF88D3E195BD2C4B0D1CCBD9F71E32233CA4F4_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	String_t* _returnValue;
	_returnValue = Vector_1_ToString_m3EBF88D3E195BD2C4B0D1CCBD9F71E32233CA4F4(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 7883
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&StringBuilder_t_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	StringBuilder_t* V_0 = NULL;
	String_t* V_1 = NULL;
	int32_t V_2 = 0;
	{
		StringBuilder_t* L_0 = (StringBuilder_t*)il2cpp_codegen_object_new(StringBuilder_t_il2cpp_TypeInfo_var);
		StringBuilder__ctor_m1D99713357DE05DAFA296633639DB55F8C30587D(L_0, NULL);
		V_0 = L_0;
		RuntimeObject* L_1 = ___1_formatProvider;
		NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* L_2;
		L_2 = NumberFormatInfo_GetInstance_m705987E5E7D3E5EC5C5DD2D088FBC9BCBA0FC31F(L_1, NULL);
		NullCheck(L_2);
		String_t* L_3;
		L_3 = NumberFormatInfo_get_NumberGroupSeparator_m0556B092AA471513B1EDC31C047712226D39BEB6_inline(L_2, NULL);
		V_1 = L_3;
		StringBuilder_t* L_4 = V_0;
		NullCheck(L_4);
		StringBuilder_t* L_5;
		L_5 = StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1(L_4, (Il2CppChar)((int32_t)60), NULL);
		V_2 = 0;
		goto IL_0053;
	}

IL_001f:
	{
		StringBuilder_t* L_6 = V_0;
		int32_t L_7 = V_2;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint64_t L_8;
		L_8 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99(__this, L_7, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		uint64_t L_9 = L_8;
		RuntimeObject* L_10 = Box(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4), &L_9);
		String_t* L_11 = ___0_format;
		RuntimeObject* L_12 = ___1_formatProvider;
		String_t* L_13;
		L_13 = UInt64_ToString_m779041C8FDD58BF8617838B00CD041788DB2F1A3((uint64_t*)UnBox(L_10, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 4)), L_11, L_12, NULL);
		NullCheck(L_6);
		StringBuilder_t* L_14;
		L_14 = StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D(L_6, L_13, NULL);
		StringBuilder_t* L_15 = V_0;
		String_t* L_16 = V_1;
		NullCheck(L_15);
		StringBuilder_t* L_17;
		L_17 = StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D(L_15, L_16, NULL);
		StringBuilder_t* L_18 = V_0;
		NullCheck(L_18);
		StringBuilder_t* L_19;
		L_19 = StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1(L_18, (Il2CppChar)((int32_t)32), NULL);
		int32_t L_20 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_20, 1));
	}

IL_0053:
	{
		int32_t L_21 = V_2;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_22;
		L_22 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_21) < ((int32_t)((int32_t)il2cpp_codegen_subtract(L_22, 1)))))
		{
			goto IL_001f;
		}
	}
	{
		StringBuilder_t* L_23 = V_0;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_24;
		L_24 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		uint64_t L_25;
		L_25 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99(__this, ((int32_t)il2cpp_codegen_subtract(L_24, 1)), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		uint64_t L_26 = L_25;
		RuntimeObject* L_27 = Box(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4), &L_26);
		String_t* L_28 = ___0_format;
		RuntimeObject* L_29 = ___1_formatProvider;
		String_t* L_30;
		L_30 = UInt64_ToString_m779041C8FDD58BF8617838B00CD041788DB2F1A3((uint64_t*)UnBox(L_27, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 4)), L_28, L_29, NULL);
		NullCheck(L_23);
		StringBuilder_t* L_31;
		L_31 = StringBuilder_Append_m08904D74E0C78E5F36DCD9C9303BDD07886D9F7D(L_23, L_30, NULL);
		StringBuilder_t* L_32 = V_0;
		NullCheck(L_32);
		StringBuilder_t* L_33;
		L_33 = StringBuilder_Append_m71228B30F05724CD2CD96D9611DCD61BFB96A6E1(L_32, (Il2CppChar)((int32_t)62), NULL);
		StringBuilder_t* L_34 = V_0;
		NullCheck((RuntimeObject*)L_34);
		String_t* L_35;
		L_35 = VirtualFuncInvoker0< String_t* >::Invoke(3, (RuntimeObject*)L_34);
		return L_35;
	}
}
IL2CPP_EXTERN_C  String_t* Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C_AdjustorThunk (RuntimeObject* __this, String_t* ___0_format, RuntimeObject* ___1_formatProvider, const RuntimeMethod* method)
{
	Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* _thisAdjusted;
	_thisAdjusted = il2cpp_codegen_get_raw_data<Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A>(__this);
	String_t* _returnValue;
	_returnValue = Vector_1_ToString_m8F20119DB8CF7117F2D6E4D165C4A843F7D3586C(_thisAdjusted, ___0_format, ___1_formatProvider, method);
	return _returnValue;
}
// Method Definition Index: 7884
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_op_Equality_mD4D4AE7733CACE50CA2FCFFFB0A16818EEC01293_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_left, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_0 = ___1_right;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_1;
		L_1 = Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27((&___0_left), L_0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_1;
	}
}
// Method Definition Index: 7885
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_op_Inequality_m4963768CF3F7944DA5E519ADB8668431198BBC36_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_left, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_0 = ___0_left;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_1 = ___1_right;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_2;
		L_2 = Vector_1_op_Equality_mD4D4AE7733CACE50CA2FCFFFB0A16818EEC01293_inline(L_0, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 11));
		return (bool)((((int32_t)L_2) == ((int32_t)0))? 1 : 0);
	}
}
// Method Definition Index: 7886
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A Vector_1_op_Explicit_mEC3EDF70D967977C9CD17D95CD3D6B52F405B08B_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_value, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	//<source_info:<no-source>:1>
	{
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* L_0 = (Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A*)(&(&___0_value)->___register);
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_1;
		memset((&L_1), 0, sizeof(L_1));
		Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606((&L_1), L_0, Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606_RuntimeMethod_var);
		return L_1;
	}
}
// Method Definition Index: 7887
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A Vector_1_Equals_m10063846A51F9D722BC0A8999A9F6B12C37988FF_gshared (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_left, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint8_t* V_0 = NULL;
	int32_t V_1 = 0;
	int8_t* V_2 = NULL;
	int32_t V_3 = 0;
	uint16_t* V_4 = NULL;
	int32_t V_5 = 0;
	int16_t* V_6 = NULL;
	int32_t V_7 = 0;
	uint32_t* V_8 = NULL;
	int32_t V_9 = 0;
	int32_t* V_10 = NULL;
	int32_t V_11 = 0;
	uint64_t* V_12 = NULL;
	int32_t V_13 = 0;
	int64_t* V_14 = NULL;
	int32_t V_15 = 0;
	float* V_16 = NULL;
	int32_t V_17 = 0;
	double* V_18 = NULL;
	int32_t V_19 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A V_20;
	memset((&V_20), 0, sizeof(V_20));
	uint64_t* G_B53_0 = NULL;
	uint64_t* G_B52_0 = NULL;
	uint64_t G_B54_0 = 0;
	uint64_t* G_B54_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B265_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B264_0 = NULL;
	uint64_t G_B266_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B266_1 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B268_0 = NULL;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B267_0 = NULL;
	uint64_t G_B269_0 = 0;
	Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A* G_B269_1 = NULL;
	{
		bool L_0;
		L_0 = Vector_get_IsHardwareAccelerated_m783509258751EBED64CBD9F387EC1BB4A15088AA(NULL);
		if (!L_0)
		{
			goto IL_0447;
		}
	}
	{
		goto IL_0068;
	}

IL_0068:
	{
		goto IL_00c6;
	}

IL_00c6:
	{
		goto IL_0133;
	}

IL_0133:
	{
		goto IL_01a0;
	}

IL_01a0:
	{
		goto IL_020d;
	}

IL_020d:
	{
		goto IL_027a;
	}

IL_027a:
	{
	}
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_1;
		L_1 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		uintptr_t L_2 = (il2cpp_codegen_conv<uintptr_t,int32_t,int32_t,false,false>(L_1,NULL));
		if ((uintptr_t)L_2 * (uintptr_t)8 > (uintptr_t)kIl2CppUIntPtrMax)
			IL2CPP_RAISE_MANAGED_EXCEPTION(il2cpp_codegen_get_overflow_exception(), method);
		intptr_t L_3 = ((intptr_t)il2cpp_codegen_multiply((intptr_t)L_2, 8));
		int8_t* L_4;
		if (L_3 == 0)
		{
			L_4 = NULL;
		}
		else
		{
			L_4 = (int8_t*)alloca(L_3);
			memset(L_4, 0, L_3);
		}
		V_12 = (uint64_t*)L_4;
		V_13 = 0;
		goto IL_02d7;
	}

IL_02a6:
	{
		uint64_t* L_5 = V_12;
		int32_t L_6 = V_13;
		intptr_t L_7 = (il2cpp_codegen_conv<intptr_t,int32_t,int32_t,false,false>(L_6,NULL));
		int32_t L_8 = V_13;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		uint64_t L_9;
		L_9 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99((&___0_left), L_8, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		int32_t L_10 = V_13;
		uint64_t L_11;
		L_11 = Vector_1_get_Item_m685EA4A01E8AF51DC7B5F78DE0583F8FD4997C99((&___1_right), L_10, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 8));
		bool L_12;
		L_12 = Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_inline(L_9, L_11, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 9));
		if (L_12)
		{
			G_B53_0 = ((uint64_t*)il2cpp_codegen_add((intptr_t)L_5, ((intptr_t)il2cpp_codegen_multiply(L_7, 8))));
			goto IL_02cb;
		}
		G_B52_0 = ((uint64_t*)il2cpp_codegen_add((intptr_t)L_5, ((intptr_t)il2cpp_codegen_multiply(L_7, 8))));
	}
	{
		int64_t L_13 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>(0,NULL));
		G_B54_0 = ((uint64_t)(L_13));
		G_B54_1 = G_B52_0;
		goto IL_02d0;
	}

IL_02cb:
	{
		uint64_t L_14;
		L_14 = ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline(NULL);
		G_B54_0 = L_14;
		G_B54_1 = G_B53_0;
	}

IL_02d0:
	{
		il2cpp_codegen_stind<int64_t>((int64_t*)G_B54_1, (int64_t)G_B54_0);
		int32_t L_15 = V_13;
		V_13 = ((int32_t)il2cpp_codegen_add(L_15, 1));
	}

IL_02d7:
	{
		int32_t L_16 = V_13;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_17;
		L_17 = Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 5));
		if ((((int32_t)L_16) < ((int32_t)L_17)))
		{
			goto IL_02a6;
		}
	}
	{
		uint64_t* L_18 = V_12;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_19;
		memset((&L_19), 0, sizeof(L_19));
		Vector_1__ctor_mBEC18AF78DE340D929AD22019717DE9ED57A4CCA((&L_19), (void*)L_18, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 12));
		return L_19;
	}

IL_0447:
	{
		il2cpp_codegen_initobj((&V_20), sizeof(Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A));
		goto IL_06e5;
	}

IL_06e5:
	{
		goto IL_097b;
	}

IL_097b:
	{
		goto IL_0ad9;
	}

IL_0ad9:
	{
		goto IL_0c37;
	}

IL_0c37:
	{
		goto IL_0cf9;
	}

IL_0cf9:
	{
		goto IL_0dbb;
	}

IL_0dbb:
	{
	}
	{
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_20 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_21 = L_20.___register;
		uint64_t L_22 = L_21.___uint64_0;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_23 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_24 = L_23.___register;
		uint64_t L_25 = L_24.___uint64_0;
		if ((((int64_t)L_22) == ((int64_t)L_25)))
		{
			G_B265_0 = (&V_20);
			goto IL_0df4;
		}
		G_B264_0 = (&V_20);
	}
	{
		int64_t L_26 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>(0,NULL));
		G_B266_0 = ((uint64_t)(L_26));
		G_B266_1 = G_B264_0;
		goto IL_0df9;
	}

IL_0df4:
	{
		uint64_t L_27;
		L_27 = ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline(NULL);
		G_B266_0 = L_27;
		G_B266_1 = G_B265_0;
	}

IL_0df9:
	{
		G_B266_1->___uint64_0 = G_B266_0;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_28 = ___0_left;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_29 = L_28.___register;
		uint64_t L_30 = L_29.___uint64_1;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_31 = ___1_right;
		Register_t483055A1DB8634BA3FBF01BB15D4E94E186A2E7A L_32 = L_31.___register;
		uint64_t L_33 = L_32.___uint64_1;
		if ((((int64_t)L_30) == ((int64_t)L_33)))
		{
			G_B268_0 = (&V_20);
			goto IL_0e1c;
		}
		G_B267_0 = (&V_20);
	}
	{
		int64_t L_34 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>(0,NULL));
		G_B269_0 = ((uint64_t)(L_34));
		G_B269_1 = G_B267_0;
		goto IL_0e21;
	}

IL_0e1c:
	{
		uint64_t L_35;
		L_35 = ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline(NULL);
		G_B269_0 = L_35;
		G_B269_1 = G_B268_0;
	}

IL_0e21:
	{
		G_B269_1->___uint64_1 = G_B269_0;
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_36;
		memset((&L_36), 0, sizeof(L_36));
		Vector_1__ctor_mEA86543744A54FCE590FDBF012FAE9037A5F1606((&L_36), (&V_20), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 13));
		return L_36;
	}
}
// Method Definition Index: 7888
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_gshared (uint64_t ___0_left, uint64_t ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0034;
	}

IL_0034:
	{
		goto IL_0068;
	}

IL_0068:
	{
		goto IL_009c;
	}

IL_009c:
	{
		goto IL_00d0;
	}

IL_00d0:
	{
		goto IL_0104;
	}

IL_0104:
	{
		goto IL_0138;
	}

IL_0138:
	{
	}
	{
		uint64_t L_0 = ___0_left;
		uint64_t L_1 = ___1_right;
		return (bool)((((int64_t)L_0) == ((int64_t)L_1))? 1 : 0);
	}
}
// Method Definition Index: 7889
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint64_t Vector_1_GetOneValue_mE2DE5D8CFC8D7A4990743C160CD1C4ED71CDA288_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0027;
	}

IL_0027:
	{
		goto IL_004e;
	}

IL_004e:
	{
		goto IL_0075;
	}

IL_0075:
	{
		goto IL_009c;
	}

IL_009c:
	{
		goto IL_00c3;
	}

IL_00c3:
	{
		goto IL_00ea;
	}

IL_00ea:
	{
	}
	{
		int64_t L_0 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>(1,NULL));
		return (uint64_t)L_0;
	}
}
// Method Definition Index: 7890
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint64_t Vector_1_GetAllBitsSetValue_m99E582A6A7DA5089B26FE42E5F8FDE26A6005ED0_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_002b;
	}

IL_002b:
	{
		goto IL_0056;
	}

IL_0056:
	{
		goto IL_0081;
	}

IL_0081:
	{
		goto IL_00ac;
	}

IL_00ac:
	{
		goto IL_00d7;
	}

IL_00d7:
	{
		goto IL_0102;
	}

IL_0102:
	{
	}
	{
		uint64_t L_0;
		L_0 = ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline(NULL);
		return L_0;
	}
}
// Method Definition Index: 7891
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Vector_1__cctor_m152F538F7C3F6DB8EA8C03902F8A68FF06A70109_gshared (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		int32_t L_0;
		L_0 = Vector_1_InitializeCount_mE29E088973A17B81B830C30831075135FC8E263A(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 14));
		((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_count = L_0;
		il2cpp_codegen_initobj((&((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_zero), sizeof(Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A));
		uint64_t L_1;
		L_1 = Vector_1_GetOneValue_mE2DE5D8CFC8D7A4990743C160CD1C4ED71CDA288_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 15));
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_2;
		memset((&L_2), 0, sizeof(L_2));
		Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6((&L_2), L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 16));
		((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_one = L_2;
		uint64_t L_3;
		L_3 = Vector_1_GetAllBitsSetValue_m99E582A6A7DA5089B26FE42E5F8FDE26A6005ED0_inline(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 17));
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_4;
		memset((&L_4), 0, sizeof(L_4));
		Vector_1__ctor_m1B5D6A9264B4450B3C14BD8FF9430354A337F2D6((&L_4), L_3, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 16));
		((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_allOnes = L_4;
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Method Definition Index: 22856
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* Widget_get_displayName_m1E7836A53BF88619C4815EA70F9243319567F0F4_inline (Widget_tE8D6AF1D7525CC84E8F2C3B73162016736A6A2FF* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		String_t* L_0 = __this->___U3CdisplayNameU3Ek__BackingField;
		return L_0;
	}
}
// Method Definition Index: 8339
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* NumberFormatInfo_get_NumberGroupSeparator_m0556B092AA471513B1EDC31C047712226D39BEB6_inline (NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* __this, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		String_t* L_0 = __this->___numberGroupSeparator;
		return L_0;
	}
}
// Method Definition Index: 7863
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint16_t ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint16_t V_0 = 0;
	{
		V_0 = (uint16_t)0;
		uintptr_t L_0 = (il2cpp_codegen_conv<uintptr_t,uint16_t*,intptr_t,false,false>((&V_0),NULL));
		il2cpp_codegen_stind<int16_t>((int16_t*)L_0, (int16_t)((int32_t)65535));
		uint16_t L_1 = V_0;
		return L_1;
	}
}
// Method Definition Index: 7867
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint64_t ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	uint64_t V_0 = 0;
	{
		int64_t L_0 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>(0,NULL));
		V_0 = L_0;
		uintptr_t L_1 = (il2cpp_codegen_conv<uintptr_t,uint64_t*,intptr_t,false,false>((&V_0),NULL));
		int64_t L_2 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>((-1),NULL));
		il2cpp_codegen_stind<int64_t>((int64_t*)L_1, (int64_t)L_2);
		uint64_t L_3 = V_0;
		return L_3;
	}
}
// Method Definition Index: 7871
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Vector_1_get_Count_m6DF09E4443FC90521D33C892BE69D32B04D85A15_gshared_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_0 = ((Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_count;
		return L_0;
	}
}
// Method Definition Index: 7879
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_Equals_mD7F4E0B493DD44E2685BC17F8D6EAD92342CBC29_gshared_inline (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		RuntimeObject* L_0 = ___0_obj;
		if (((RuntimeObject*)IsInstSealed((RuntimeObject*)L_0, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0))))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject* L_1 = ___0_obj;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_2;
		L_2 = Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8(__this, ((*(Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489*)UnBox(L_1, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0)))), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_2;
	}
}
// Method Definition Index: 7888
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_ScalarEquals_m4E13E30219B0D2AADB58AD6E5CB2B54B9FCBFAAE_gshared_inline (uint16_t ___0_left, uint16_t ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0034;
	}

IL_0034:
	{
		goto IL_0068;
	}

IL_0068:
	{
	}
	{
		uint16_t L_0 = ___0_left;
		uint16_t L_1 = ___1_right;
		return (bool)((((int32_t)L_0) == ((int32_t)L_1))? 1 : 0);
	}
}
// Method Definition Index: 7884
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_op_Equality_mB42F3DAE52C3BC7579B302E623196C45A5DEAC6B_gshared_inline (Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___0_left, Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Vector_1_tACF5C606E327928B31CCD8E09C9224DCA7065489 L_0 = ___1_right;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_1;
		L_1 = Vector_1_Equals_m729FD34A0F43A7C8A8DF285BCED9B5B31D579FE8((&___0_left), L_0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_1;
	}
}
// Method Definition Index: 7889
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint16_t Vector_1_GetOneValue_m7E814AFD17E4D390C12EF731DA01203D262D9953_gshared_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0027;
	}

IL_0027:
	{
		goto IL_004e;
	}

IL_004e:
	{
	}
	{
		return (uint16_t)1;
	}
}
// Method Definition Index: 7890
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint16_t Vector_1_GetAllBitsSetValue_m854DE079EA89F97089D3EF29D7C31F081F420580_gshared_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_002b;
	}

IL_002b:
	{
		goto IL_0056;
	}

IL_0056:
	{
	}
	{
		uint16_t L_0;
		L_0 = ConstantHelper_GetUInt16WithAllBitsSet_mD3E13D933A06059499F0E0CBE6798D72D175464A_inline(NULL);
		return L_0;
	}
}
// Method Definition Index: 7871
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Vector_1_get_Count_mC75C8C6E913E7FF8A3D10467D6DADE41711EF3CC_gshared_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		int32_t L_0 = ((Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1)))->___s_count;
		return L_0;
	}
}
// Method Definition Index: 7879
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_Equals_mE275DCDE4DC3B6FB30AB80ACEAC8363207BA9BEC_gshared_inline (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		RuntimeObject* L_0 = ___0_obj;
		if (((RuntimeObject*)IsInstSealed((RuntimeObject*)L_0, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0))))
		{
			goto IL_000a;
		}
	}
	{
		return (bool)0;
	}

IL_000a:
	{
		RuntimeObject* L_1 = ___0_obj;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_2;
		L_2 = Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27(__this, ((*(Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A*)UnBox(L_1, il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 0)))), il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_2;
	}
}
// Method Definition Index: 7888
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_ScalarEquals_m73081D1B852400C74618D0A814BBED2FE272175D_gshared_inline (uint64_t ___0_left, uint64_t ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0034;
	}

IL_0034:
	{
		goto IL_0068;
	}

IL_0068:
	{
		goto IL_009c;
	}

IL_009c:
	{
		goto IL_00d0;
	}

IL_00d0:
	{
		goto IL_0104;
	}

IL_0104:
	{
		goto IL_0138;
	}

IL_0138:
	{
	}
	{
		uint64_t L_0 = ___0_left;
		uint64_t L_1 = ___1_right;
		return (bool)((((int64_t)L_0) == ((int64_t)L_1))? 1 : 0);
	}
}
// Method Definition Index: 7884
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Vector_1_op_Equality_mD4D4AE7733CACE50CA2FCFFFB0A16818EEC01293_gshared_inline (Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___0_left, Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A ___1_right, const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		Vector_1_t566D05A9DE75BCD8F12F1E09AC3F8A4BC01BF92A L_0 = ___1_right;
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		bool L_1;
		L_1 = Vector_1_Equals_mAE01D42B31EB54893DC4DB1BE8A99216AF784C27((&___0_left), L_0, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 7));
		return L_1;
	}
}
// Method Definition Index: 7889
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint64_t Vector_1_GetOneValue_mE2DE5D8CFC8D7A4990743C160CD1C4ED71CDA288_gshared_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_0027;
	}

IL_0027:
	{
		goto IL_004e;
	}

IL_004e:
	{
		goto IL_0075;
	}

IL_0075:
	{
		goto IL_009c;
	}

IL_009c:
	{
		goto IL_00c3;
	}

IL_00c3:
	{
		goto IL_00ea;
	}

IL_00ea:
	{
	}
	{
		int64_t L_0 = (il2cpp_codegen_conv<int64_t,int32_t,int32_t,false,false>(1,NULL));
		return (uint64_t)L_0;
	}
}
// Method Definition Index: 7890
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR uint64_t Vector_1_GetAllBitsSetValue_m99E582A6A7DA5089B26FE42E5F8FDE26A6005ED0_gshared_inline (const RuntimeMethod* method) 
{
	//<source_info:<no-source>:1>
	{
		goto IL_002b;
	}

IL_002b:
	{
		goto IL_0056;
	}

IL_0056:
	{
		goto IL_0081;
	}

IL_0081:
	{
		goto IL_00ac;
	}

IL_00ac:
	{
		goto IL_00d7;
	}

IL_00d7:
	{
		goto IL_0102;
	}

IL_0102:
	{
	}
	{
		uint64_t L_0;
		L_0 = ConstantHelper_GetUInt64WithAllBitsSet_mB7F3E046EE6B1B20C552BF7CF619416E239A5A96_inline(NULL);
		return L_0;
	}
}
