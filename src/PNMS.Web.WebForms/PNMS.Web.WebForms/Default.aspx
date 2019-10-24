<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PNMS.Web.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row" id="categories">
        <div class="gallery col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <h1 class="gallery-title">News Categories</h1>
        </div>
        <br/>

        </div>
    </div>
    <script>
        $.ajax({
            type: "GET",
            url: "http://localhost:52530/api/category",
            success: function (data) {
                $.each(data, function (key, value) {
                    var item = '<div class="gallery_product col-lg-4 col-md-4 col-sm-4 col-xs-6 filter ' + value.Id + '">';
                    item += '<a href="Category.aspx?ctg=' + value.Id +'">';
                    item += '<img src="http://localhost:52530/' + value.ImageUrl + '" class="img-responsive gallery_product_picture">';
                    item += '<p class="gallery_product_text">' + value.Name[0].toUpperCase() + value.Name.slice(1) + '</p><a></div>'
                    $("#categories").append(item);
                });
            },
            dataType: 'json'
        });
    </script>
</asp:Content>
