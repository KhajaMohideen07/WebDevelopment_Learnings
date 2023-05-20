<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BookIssueReturnPage.aspx.cs" Inherits="ELibraryManagement.BookIssueReturnPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-5">
                <div class="card">
                    <div class="card-body">

                        <div class="row">
                            <div class="col">
                                <center>
                                    <h3>Book Issuing/Return</h3>
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <center>
                                    <img width="100px" src="imgs/books.png" />
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>

                        <div class="row">
                         
                            <div class="col-md-6">
                                <label>Member Id</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox2" runat="server" placeholder="Member Id"></asp:TextBox>
                                </div>
                            </div>
                               <div class="col-md-6">
                                <label>Book ID</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox1" runat="server" placeholder="Book ID"></asp:TextBox>
                                        <asp:Button class="btn btn-primary " ID="Button2" runat="server" Text="Go" />
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="row">
                         
                            <div class="col-md-6">
                                <label>Member Name</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox3" runat="server" placeholder="Member Name" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                               <div class="col-md-6">
                                <label>Book Name</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox4" runat="server" placeholder="Book Name" ReadOnly="True"></asp:TextBox>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
                            <div class="row">
                         
                            <div class="col-md-6">
                                <label>Start Date</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox5" runat="server" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                               <div class="col-md-6">
                                <label>End Date</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox6" runat="server"  TextMode="Date"></asp:TextBox>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>


                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button class="btn btn-primary w-100 " ID="Button1" runat="server" Text="Issue" />
                            </div>
                            <div class="col-md-6">
                                <asp:Button class="btn btn-success w-100 " ID="Button4" runat="server" Text="Return" />
                            </div>
                        </div>


                    </div>
                </div>
                <a href="homepage.aspx"><< Back to Home</a><br>
                <br>
            </div>

            <div class="col-md-7">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <center>
                                        <h3>Issued Book List</h3>

                                    </center>
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <asp:GridView ID="GridView1" class="table table-striped table-bordered" runat="server"></asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
