<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserManagementPage.aspx.cs" Inherits="ELibraryManagement.UserManagementPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h3>Member Details</h3>
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <center>
                                    <img width="100px" src="imgs/generaluser.png" />
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <hr>
                            </div>
                        </div>

                        <div class="row">

                               <div class="col-md-3">
                                <label>Member ID</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <div class="input-group">
                                        <asp:TextBox Class="form-control" ID="TextBox1" runat="server" placeholder="Member ID"></asp:TextBox>
                                        <asp:LinkButton class="btn btn-primary" ID="LinkButton1" runat="server">Go</asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <label>Full Name</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox2" runat="server" placeholder="Full Name" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>

                               <div class="col-md-6">
                                <label>Account Status</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <div class="input-group">
                                        <asp:TextBox CssClass="form-control" ID="TextBox7" runat="server" placeholder="Account Status" ReadOnly="True"></asp:TextBox>
                                        <asp:LinkButton class="btn btn-primary" ID="LinkButton2" runat="server"><i class="fa-solid fa-circle-check"></i></asp:LinkButton>
                                        <asp:LinkButton class="btn btn-warning" ID="LinkButton3" runat="server"><i class="fa-solid fa-circle-pause"></i></asp:LinkButton>
                                        <asp:LinkButton class="btn btn-danger" ID="LinkButton4" runat="server"><i class="fa-solid fa-circle-xmark"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                         
                        </div>

                        <div class="row">

                            <div class="col-md-3">
                                <label>DOB</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox3" runat="server" placeholder="DOB" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                           <div class="col-md-4">
                                <label>Contact Number</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox4" runat="server" placeholder="Contact Number" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <label>Email</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox8" runat="server" placeholder="Email" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-12">
                                <label>Full Address</label>
                                <div class="form-group" style="margin-bottom: 10px">
                                    <asp:TextBox CssClass="form-control" ID="TextBox5" runat="server" placeholder="Enter Your Address" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>
   
                        </div>


                        <br />
                        <div class="row">
                            <div class="col-md-8 mx-auto">
                                <asp:Button class="btn btn-danger w-100 " ID="Button1" runat="server" Text="Delete User Permanantly" />
                            </div>

                        </div>


                    </div>
                </div>
                <a href="homepage.aspx"><< Back to Home</a><br>
                <br>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <center>
                                        <h3>Member List</h3>

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
