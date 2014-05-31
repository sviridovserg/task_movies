﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Classic Movies Home</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Movies in Library</h3>
        <div style="float:right">
            <button type="submit" class="btn btn-info" ng-click="addMovie()" ng-show="loggedIn">Add Movie</button>
        </div>
    </div>
    <form class="form-search">
        <select   ng-model="searchField" ng-options="f.value for f in fields">
        </select>
      <input type="text" class="input-medium search-query"  ng-model="searchVal"/>
      <button type="submit" class="btn btn-success" ng-click="search(searchField, searchVal)" ng-disabled="!searchField || !searchVal">Search</button>
    </form>
    <div>
        <table class="table table-hover">
            <thead>
                <tr>
                    <th class="t-sorted-header" ng-click="sort('id', asc)">
                        <span>#</span><span><i ng-show="sortField=='id'" ng-class="{'icon-arrow-down':asc, 'icon-arrow-up':!asc}"></i></span>
                    </th>
                    <th class="t-sorted-header" ng-click="sort('title', asc)">
                        <span>Title</span><span><i ng-show="sortField=='title'" ng-class="{'icon-arrow-down':asc, 'icon-arrow-up':!asc}"></i></span>
                    </th>
                    <th class="t-sorted-header" ng-click="sort('releaseYear', asc)">
                        <span>Year</span><span><i ng-show="sortField=='releaseYear'" ng-class="{'icon-arrow-down':asc, 'icon-arrow-up':!asc}"></i></span>
                    </th>
                    <th class="t-sorted-header" ng-click="sort('rating', asc)">
                        <span>Rating</span><span><i ng-show="sortField=='rating'" ng-class="{'icon-arrow-down':asc, 'icon-arrow-up':!asc}"></i></span>
                    </th>
                    <th class="t-sorted-header" ng-click="sort('genre', asc)">
                        <span>Genre</span><span><i ng-show="sortField=='genre'" ng-class="{'icon-arrow-down':asc, 'icon-arrow-up':!asc}"></i></span>
                    </th>
                    <th class="t-sorted-header" ng-click="sort('classification', asc)">
                        <span>Classification</span><span><i ng-show="sortField=='classification'" ng-class="{'icon-arrow-down':asc, 'icon-arrow-up':!asc}"></i></span>
                    </th>
                    <th>Actors</th>
                </tr>
            </thead>
            <tbody>
                <tr ng-repeat="m in movies | orderBy:sortField:asc">
                    <td>{{m.id}}</td>
                    <td><a href="" ng-click="editMovie(m)">{{m.title}}</a></td>
                    <td>{{m.releaseYear}}</td>
                    <td>{{m.rating}}</td>
                    <td>{{m.genre}}</td>
                    <td>{{m.classification}}</td>
                    <td>
                        {{m.cast.join(', ')}}
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
