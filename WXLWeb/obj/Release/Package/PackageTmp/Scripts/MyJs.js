 //多说公共JS代码 start (一个网页只需插入一次) 
    var duoshuoQuery = { short_name: "wxiaolong" };
    function duoshuoMessageBoard() {
        var ds = document.createElement('script');
        ds.type = 'text/javascript'; ds.async = true;
        ds.src = (document.location.protocol == 'https:' ? 'https:' : 'http:') + '//static.duoshuo.com/embed.js';
        ds.charset = 'UTF-8';
        (document.getElementsByTagName('head')[0]
         || document.getElementsByTagName('body')[0]).appendChild(ds);
    };
//多说公共JS代码 end 

function active_li6() {
    var a = $("#nav_ul a");
    a[5].setAttribute("class", "active_li");
}
//主页
function active_li1() {
    var a = $("#nav_ul a");
    a[0].setAttribute("class", "active_li");
}
//关于我
function active_li2() {
    var a = $("#nav_ul a");
    a[1].setAttribute("class", "active_li");
}
//技术分享
function active_li4() {
    var a = $("#nav_ul a");
    a[3].setAttribute("class", "active_li");
}