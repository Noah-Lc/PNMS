<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PNMS.Web.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row" id="categories">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <h1 class="gallery-title"></h1>
        </div>

        </div>
    </div>
<script>
    var category = '';
    $.ajax({
        type: "GET",
        url: "http://localhost:52530/api/category/<%= Request.QueryString.Get("ctg") %>",
        success: function (data) {
            $(".gallery-title").html(data.Name[0].toUpperCase() + data.Name.slice(1));
            $.ajax({
                type: "GET",
                url: "http://localhost:52530/api/news?category=" + data.Name,
                success: function (data) {
                    $.each(data, function (key, value) {
                        var item = '<div class="col-md-3 col-sm-6 col-xs-12"><div class="post"><div class="post-img-content"><img src="/Content/empty.png" class="img-responsive" />';
                        item += '<span class="post-title"><b>' + value.Name + '</b></span></div>';
                        item += '<div class="content"><div class="author">By <b>Admin</b> |<time datetime="' + value.NormalDate + '">' + value.LongDate + '</time></div>';
                        item += '<div>' + value.Text.substring(0, 200) + '</div><div><a href="#" class="btn btn-success btn-sm">Read more</a></div></div></div></div>';
                        $("#categories").append(item);
                    });
                },
                dataType: 'json'
            });
        },
        dataType: 'json'
    });
</script>
</asp:Content>
