<%@ Page Title="Categories" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="PNMS.Web.WebForms.Categories" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div id="ctg">

    </div>
    <script>
        $.ajax({
            type: "GET",
            url: "http://localhost:52530/api/category",
            contentType: 'application/x-www-form-urlencoded',
            headers: {'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFkbWluQGRlbW8uY29tIiwibmFtZWlkIjoiMSIsInVuaXF1ZV9uYW1lIjoiYWRtaW4iLCJuYmYiOjE1NzE4NzE1MjgsImV4cCI6MTU3MTk1NzkyOCwiaWF0IjoxNTcxODcxNTI4LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNTMwL2FwaSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTI1MzAvIn0.LaObQYZ7RGCo7yC3I_Cap8hp4GPXXOO23QmHat320rs'},
            success: function (data) {
            $.each(data, function (key, value) {
                var item = "<p>" + value.Name + "</p><img src='http://localhost:52530/" + value.ImageUrl + "'/>";
                    $("#ctg").append(item);
                });
            },
        dataType: 'json'
    });
</script>
</asp:Content>
