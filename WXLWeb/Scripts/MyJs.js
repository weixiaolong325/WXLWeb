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

function active_li_messageboard() {
    var a = $("#nav_ul_messageboard");
    a[0].setAttribute("class", "active_li");
}
//主页
function active_li_index() {
    var a = $("#nav_ul_index");
    a[0].setAttribute("class", "active_li");
    $('.carousel').carousel()
}
//关于我
function active_li_aboutme() {
    var a = $("#nav_ul_aboutme");
    a[0].setAttribute("class", "active_li");
}
//生活记录
function active_li_life() {
    var a = $("#nav_ul_life");
    a[0].setAttribute("class", "active_li");
}
//技术分享
function active_li_skill() {
    var a = $("#nav_ul_skill");
    a[0].setAttribute("class", "active_li");
}
//学海无涯
function active_li_learn() {
    var a = $("#nav_ul_learn");
    a[0].setAttribute("class", "active_li");
}