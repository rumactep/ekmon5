﻿using System;
using System.Threading.Tasks;
using smartlink;
using Xunit;
using Xunit.Abstractions;

namespace smartlinkTests;

public class ReadAnalogInputsTestIElektronikonClient {
    private readonly ITestOutputHelper _testOutputHelper;

    public ReadAnalogInputsTestIElektronikonClient(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    private const string Question1 =
        "201001201004201101201104201201201204201301201304201401201404201501201504201601201604201701201704201801201804201901201904201a01201a04201b01201b04201c01201c04201d01201d04201e01201e04201f01201f04202001202004202101202104202201202204202301202304202401202404202501202504202601202604202701202704202801202804202901202904202a01202a04202b01202b04202c01202c04202d01202d04202e01202e04202f01202f04203001203004203101203104203201203204203301203304203401203404203501203504203601203604203701203704203801203804203901203904203a01203a04203b01203b04203c01203c04203d01203d04203e01203e04203f01203f04204001204004204101204104204201204204204301204304204401204404204501204504204601204604204701204704204801204804204901204904204a01204a04204b01204b04204c01204c04204d01204d04204e01204e04204f01204f04205001205004205101205104205201205204205301205304205401205404205501205504205601205604205701205704205801205804205901205904205a01205a04205b01205b04205c01205c04205d01205d04205e01205e04205f01205f04206001206004206101206104206201206204206301206304206401206404206501206504206601206604206701206704206801206804206901206904206a01206a04206b01206b04206c01206c04206d01206d04206e01206e04206f01206f04207001207004207101207104207201207204207301207304207401207404207501207504207601207604207701207704207801207804207901207904207a01207a04207b01207b04207c01207c04207d01207d04207e01207e04207f01207f04208001208004208101208104208201208204208301208304208401208404208501208504208601208604208701208704208801208804208901208904208a01208a04208b01208b04208c01208c04208d01208d04208e01208e04208f01208f0420b00120b10120b20120b30120b40120b50120b60120b70120b80120b90120ba0120bb0120bc0120bd0120be0120bf0120c00120c10120c20120c30120c40120c50120c60120c70120c80120c90120ca0120cb0120cc0120cd0120ce0120cf0120d00120d10120d20120d30120d40120d50120d60120d70120d80120d90120da0120db0120dc0120dd0120de0120df0120e00120e10120e20120e30120e40120e50120e60120e70120e80120e90120ea0120eb0120ec0120ed0120ee0120ef0120f00120f10120f20120f30120f40120f50120f60120f70120f80120f90120fa0120fb0120fc0120fd0120fe0120ff0126070126070226070326070426070526070626070726070826070926070a26070b26070c26070d26070e26070f26071026071126071226071326071426071526071626071726071826071926071a26071b26071c26071d26071e26071f26072026072126072226072326072426072526072626072726072826072926072a26072b26072c26072d26072e26072f26073026073126073226073326073426073526073626073726073826073926073a26073b26073c26073d26073e26073f26074026074126074226074326074426074526074626074726074826074926074a26074b26074c26074d26074e26074f26075026075126075226075326075426075526075626075726075826075926075a26075b26075c26075d26075e26075f26076026076126076226076326076426076526076626076726076826076926076a26076b26076c26076d26076e26076f26077026077126077226077326077426077526077626077726077826077926077a26077b26077c26077d26077e26077f26078026078126078226078326078426078526078626078726078826078926078a26078b26078c26078d26078e26078f26079026079126079226079326079426079526079626079726079826079926079a26079b26079c26079d26079e26079f2607a02607a12607a22607a32607a42607a52607a62607a72607a82607a92607aa2607ab2607ac2607ad2607ae2607af2607b02607b12607b22607b32607b42607b52607b62607b72607b82607b92607ba2607bb2607bc2607bd2607be2607bf2607c02607c12607c22607c32607c42607c52607c62607c72607c82607c92607ca2607cb2607cc2607cd2607ce2607cf2607d02607d12607d22607d32607d42607d52607d62607d72607d82607d92607da2607db2607dc2607dd2607de2607df2607e02607e12607e22607e32607e42607e52607e62607e72607e82607e92607ea2607eb2607ec2607ed2607ee2607ef2607f02607f12607f22607f32607f42607f52607f62607f72607f82607f92607fa2607fb2607fc2607fd2607fe2607ff268101268107268201268207268301268307268401268407268501268507268601268607268701268707268801268807210001210101210201210301210401210501210601210701210801210901210a01210b01210c01210d01210e01210f01211001211101211201211301211401211501211601211701211801211901211a01211b01211c01211d01211e01211f01212001212101212201212301212401212501212601212701212801212901212a01212b01212c01212d01212e01212f01213001213101213201213301213401213501213601213701213801213901213a01213b01213c01213d01213e01213f01214001214101214201214301214401214501214601214701214801214901214a01214b01214c01214d01214e01214f01209001209003209101209103209201209203209301209303209401209403209501209503209601209603209701209703209801209803209901209903209a01209a03209b01209b03209c01209c03209d01209d03209e01209e03209f01209f0320a00120a00320a10120a10320a20120a20320a30120a30320a40120a40320a50120a50320a60120a60320a70120a70320a80120a80320a90120a90320aa0120aa0320ab0120ab0320ac0120ac0320ad0120ad0320ae0120ae0320af0120af03268101268107268201268207268301268307268401268407268501268507268601268607268701268707268801268807230001230101230201230301230401230501230601230701230801230901230a01230b01230c01230d01230e01230f01231001231101231201231301231401231501231601231701231801231901231a01231b01231c01231d01231e01231f01232001232101232201232301232401232501232601232701232801232901232a01232b01232c01232d01232e01232f01233001233101233201233301233401233501233601233701233801233901233a01233b01233c01233d01233e01233f01234001234101234201234301234401234501234601234701234801234901234a01234b01234c01234d01234e01234f01235001235101235201235301235401235501235601235701235801235901235a01235b01235c01235d01235e01235f01236001236101236201236301236401236501236601236701236801236901236a01236b01236c01236d01236e01236f01237001237101237201237301237401237501237601237701237801237901237a01237b01237c01237d01237e01237f01238001238101238201238301238401238501238601238701238801238901238a01238b01238c01238d01238e01238f01239001239101239201239301239401239501239601239701239801239901239a01239b01239c01239d01239e01239f0123a00123a10123a20123a30123a40123a50123a60123a70123a80123a90123aa0123ab0123ac0123ad0123ae0123af0123b00123b10123b20123b30123b40123b50123b60123b70123b80123b90123ba0123bb0123bc0123bd0123be0123bf0123c00123c10123c20123c30123c40123c50123c60123c70123c80123c90123ca0123cb0123cc0123cd0123ce0123cf0123d00123d10123d20123d30123d40123d50123d60123d70123d80123d90123da0123db0123dc0123dd0123de0123df0123e00123e10123e20123e30123e40123e50123e60123e70123e801";

    private const string Question2 =
        "23e90123ea0123eb0123ec0123ed0123ee0123ef0123f00123f10123f20123f30123f40123f50123f60123f70123f80123f90123fa0123fb0123fc0123fd0123fe0123ff01240001240101240201240301240401240501240601240701240801240901240a01240b01240c01240d01240e01240f01241001241101241201241301241401241501241601241701241801241901241a01241b01241c01241d01241e01241f01242001242101242201242301242401242501242601242701242801242901242a01242b01242c01242d01242e01242f01243001243101243201243301243401243501243601243701243801243901243a01243b01243c01243d01243e01243f01244001244101244201244301244401244501244601244701244801244901244a01244b01244c01244d01244e01244f01245001245101245201245301245401245501245601245701245801245901245a01245b01245c01245d01245e01245f01246001246101246201246301246401246501246601246701246801246901246a01246b01246c01246d01246e01246f01247001247101247201247301247401247501247601247701247801247901247a01247b01247c01247d01247e01215001215003215101215103215201215203215301215303215401215403215501215503215601215603215701215703215801215803215901215903215a01215a03215b01215b03215c01215c03215d01215d03215e01215e03215f01215f03216001216003216101216103216201216203216301216303216401216403216501216503216601216603216701216703216801216803216901216903216a01216a03216b01216b03216c01216c03216d01216d03216e01216e03216f01216f03256001256101256201256301256401256501256601256701256801256901256a01256b01256c01256d01256e01256f0131130131130331130431130531130731130831130931130a31130b31130c31130d31130e31130f31131031131131131231131331131431131531131631131731131831131931131a31131b31131c31131d31131e31131f31132031132131132231132331132431132531132631132731132831132931132a31140131140231140331140431140531140631140731140831140931140a31140b31140c31140d31140e31140f31141031141131141226020126020226020326020426020526020626020726020826020926020a26020b26020c26020d26020e26020f260210260211260212260213260214200101";

    // "201001201004201101201104201201201204201301201304201401201404201501201504201601201604201701201704201801201804201901201904201a01201a04201b01201b04201c01201c04201d01201d04201e01201e04201f01201f04202001202004202101202104202201202204202301202304202401202404202501202504202601202604202701202704202801202804202901202904202a01202a04202b01202b04202c01202c04202d01202d04202e01202e04202f01202f04203001203004203101203104203201203204203301203304203401203404203501203504203601203604203701203704203801203804203901203904203a01203a04203b01203b04203c01203c04203d01203d04203e01203e04203f01203f04204001204004204101204104204201204204204301204304204401204404204501204504204601204604204701204704204801204804204901204904204a01204a04204b01204b04204c01204c04204d01204d04204e01204e04204f01204f04205001205004205101205104205201205204205301205304205401205404205501205504205601205604205701205704205801205804205901205904205a01205a04205b01205b04205c01205c04205d01205d04205e01205e04205f01205f04206001206004206101206104206201206204206301206304206401206404206501206504206601206604206701206704206801206804206901206904206a01206a04206b01206b04206c01206c04206d01206d04206e01206e04206f01206f04207001207004207101207104207201207204207301207304207401207404207501207504207601207604207701207704207801207804207901207904207a01207a04207b01207b04207c01207c04207d01207d04207e01207e04207f01207f04208001208004208101208104208201208204208301208304208401208404208501208504208601208604208701208704208801208804208901208904208a01208a04208b01208b04208c01208c04208d01208d04208e01208e04208f01208f0420b00120b10120b20120b30120b40120b50120b60120b70120b80120b90120ba0120bb0120bc0120bd0120be0120bf0120c00120c10120c20120c30120c40120c50120c60120c70120c80120c90120ca0120cb0120cc0120cd0120ce0120cf0120d00120d10120d20120d30120d40120d50120d60120d70120d80120d90120da0120db0120dc0120dd0120de0120df0120e00120e10120e20120e30120e40120e50120e60120e70120e80120e90120ea0120eb0120ec0120ed0120ee0120ef0120f00120f10120f20120f30120f40120f50120f60120f70120f80120f90120fa0120fb0120fc0120fd0120fe0120ff0126070126070226070326070426070526070626070726070826070926070a26070b26070c26070d26070e26070f26071026071126071226071326071426071526071626071726071826071926071a26071b26071c26071d26071e26071f26072026072126072226072326072426072526072626072726072826072926072a26072b26072c26072d26072e26072f26073026073126073226073326073426073526073626073726073826073926073a26073b26073c26073d26073e26073f26074026074126074226074326074426074526074626074726074826074926074a26074b26074c26074d26074e26074f26075026075126075226075326075426075526075626075726075826075926075a26075b26075c26075d26075e26075f26076026076126076226076326076426076526076626076726076826076926076a26076b26076c26076d26076e26076f26077026077126077226077326077426077526077626077726077826077926077a26077b26077c26077d26077e26077f26078026078126078226078326078426078526078626078726078826078926078a26078b26078c26078d26078e26078f26079026079126079226079326079426079526079626079726079826079926079a26079b26079c26079d26079e26079f2607a02607a12607a22607a32607a42607a52607a62607a72607a82607a92607aa2607ab2607ac2607ad2607ae2607af2607b02607b12607b22607b32607b42607b52607b62607b72607b82607b92607ba2607bb2607bc2607bd2607be2607bf2607c02607c12607c22607c32607c42607c52607c62607c72607c82607c92607ca2607cb2607cc2607cd2607ce2607cf2607d02607d12607d22607d32607d42607d52607d62607d72607d82607d92607da2607db2607dc2607dd2607de2607df2607e02607e12607e22607e32607e42607e52607e62607e72607e82607e92607ea2607eb2607ec2607ed2607ee2607ef2607f02607f12607f22607f32607f42607f52607f62607f72607f82607f92607fa2607fb2607fc2607fd2607fe2607ff268101268107268201268207268301268307268401268407268501268507268601268607268701268707268801268807210001210101210201210301210401210501210601210701210801210901210a01210b01210c01210d01210e01210f01211001211101211201211301211401211501211601211701211801211901211a01211b01211c01211d01211e01211f01212001212101212201212301212401212501212601212701212801212901212a01212b01212c01212d01212e01212f01213001213101213201213301213401213501213601213701213801213901213a01213b01213c01213d01213e01213f01214001214101214201214301214401214501214601214701214801214901214a01214b01214c01214d01214e01214f01209001209003209101209103209201209203209301209303209401209403209501209503209601209603209701209703209801209803209901209903209a01209a03209b01209b03209c01209c03209d01209d03209e01209e03209f01209f0320a00120a00320a10120a10320a20120a20320a30120a30320a40120a40320a50120a50320a60120a60320a70120a70320a80120a80320a90120a90320aa0120aa0320ab0120ab0320ac0120ac0320ad0120ad0320ae0120ae0320af0120af03268101268107268201268207268301268307268401268407268501268507268601268607268701268707268801268807230001230101230201230301230401230501230601230701230801230901230a01230b01230c01230d01230e01230f01231001231101231201231301231401231501231601231701231801231901231a01231b01231c01231d01231e01231f01232001232101232201232301232401232501232601232701232801232901232a01232b01232c01232d01232e01232f01233001233101233201233301233401233501233601233701233801233901233a01233b01233c01233d01233e01233f01234001234101234201234301234401234501234601234701234801234901234a01234b01234c01234d01234e01234f01235001235101235201235301235401235501235601235701235801235901235a01235b01235c01235d01235e01235f01236001236101236201236301236401236501236601236701236801236901236a01236b01236c01236d01236e01236f01237001237101237201237301237401237501237601237701237801237901237a01237b01237c01237d01237e01237f01238001238101238201238301238401238501238601238701238801238901238a01238b01238c01238d01238e01238f01239001239101239201239301239401239501239601239701239801239901239a01239b01239c01239d01239e01239f0123a00123a10123a20123a30123a40123a50123a60123a70123a80123a90123aa0123ab0123ac0123ad0123ae0123af0123b00123b10123b20123b30123b40123b50123b60123b70123b80123b90123ba0123bb0123bc0123bd0123be0123bf0123c00123c10123c20123c30123c40123c50123c60123c70123c80123c90123ca0123cb0123cc0123cd0123ce0123cf0123d00123d10123d20123d30123d40123d50123d60123d70123d80123d90123da0123db0123dc0123dd0123de0123df0123e00123e10123e20123e30123e40123e50123e60123e70123e80123e90123ea0123eb0123ec0123ed0123ee0123ef0123f00123f10123f20123f30123f40123f50123f60123f70123f80123f90123fa0123fb0123fc0123fd0123fe0123ff01240001240101240201240301240401240501240601240701240801240901240a01240b01240c01240d01240e01240f01241001241101241201241301241401241501241601241701241801241901241a01241b01241c01241d01241e01241f01242001242101242201242301242401242501242601242701242801242901242a01242b01242c01242d01242e01242f01243001243101243201243301243401243501243601243701243801243901243a01243b01243c01243d01243e01243f01244001244101244201244301244401244501244601244701244801244901244a01244b01244c01244d01244e01244f01245001245101245201245301245401245501245601245701245801245901245a01245b01245c01245d01245e01245f01246001246101246201246301246401246501246601246701246801246901246a01246b01246c01246d01246e01246f01247001247101247201247301247401247501247601247701247801247901247a01247b01247c01247d01247e01215001215003215101215103215201215203215301215303215401215403215501215503215601215603215701215703215801215803215901215903215a01215a03215b01215b03215c01215c03215d01215d03215e01215e03215f01215f03216001216003216101216103216201216203216301216303216401216403216501216503216601216603216701216703216801216803216901216903216a01216a03216b01216b03216c01216c03216d01216d03216e01216e03216f01216f03256001256101256201256301256401256501256601256701256801256901256a01256b01256c01256d01256e01256f0131130131130331130431130531130731130831130931130a31130b31130c31130d31130e31130f31131031131131131231131331131431131531131631131731131831131931131a31131b31131c31131d31131e31131f31132031132131132231132331132431132531132631132731132831132931132a31140131140231140331140431140531140631140731140831140931140a31140b31140c31140d31140e31140f31141031141131141226020126020226020326020426020526020626020726020826020926020a26020b26020c26020d26020e26020f260210260211260212260213260214200101";
    private const string QuestionFull = Question1 + Question2;

    private const string Answer1 =
        "01FD00010114003CXX03F00101000500C8XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX083401010839010108350001XXXXXX084C0201XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0A8C00010A8D00010A8E01010A9101010A9200010A9300010A9400010A9500010A9600010AD801000AD901010A9002010A8F00010AEA00000AEB00000AE600001C9201001C9301001C9601001C9801001C9B01001C9C0000XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0906040100000001XXXXXXXXXXXXXX09050001XX0909000109020001X090300010901000109060001XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0906040100000001XXXXXXXXXXXXXXXX0B9F00011CD90401XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0BAF0001XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

    private const string Answer2 =
        "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064XXXXXXXXXXXXXX000007D00000223800000FA00000447000001F400000000000005DC00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001F40000000001000002";

    //private const string AnswerFull = Answer1 + Answer2;

    [Fact]
    public void TestGetRequestString4() {
        string questionString = ElektronikonReader.GetElektronikonRequest4().GetRequestString();
        Assert.Equal(QuestionFull, questionString);
    }

    [Fact]
    public async void TestSendReceive() {
        var reader = new ElektronikonReader();
        var client = new TestElektronikonClient();
        var request = await reader.Run(client);
        Assert.Equal(1, client.Ask1);
        Assert.Equal(1, client.Ask2);
        Assert.Equal(0, client.AskOther);
        _testOutputHelper.WriteLine(request.GetDataString());
    }

    public class TestElektronikonClient : IElektronikonClient {
        public int Ask1;
        public int Ask2;
        public int AskOther;

        public Task<string> AskAsync(string questionsString) {
            if (questionsString == Question1) {
                Ask1++;
                return Task.FromResult(Answer1);
            }

            if (questionsString == Question2) {
                Ask2++;
                return Task.FromResult(Answer2);
            }

            AskOther++;
            return Task.FromResult("X");
        }
    }
}