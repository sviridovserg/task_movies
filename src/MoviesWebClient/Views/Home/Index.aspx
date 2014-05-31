<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Classic Movies Home</h1>
            </hgroup>
            <p>
               Please <a href="<%:Url.Action("Login", "Account") %>">login</a> to browse through our movies
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>What do we offer:</h3>
    <ol class="round">
        <li class="one">
            <h5>Renting moview</h5>
           you can find all the latest block-busters in here. Come and visit our stores or order online.
            <a href="http://google.com">Learn more…</a>
        </li>

        <li class="two">
            <h5>Selling your favourite movies</h5>
            You can alwasy buy the movie you liked to enjoy it over an over again
            <a href="http://google.com">Learn more…</a>
        </li>

        <li class="three">
            <h5>Membership discounts and specials</h5>
            Yep, this is what we do
            <a href="http://google.com">Learn more…</a>
        </li>
    </ol>
</asp:Content>
