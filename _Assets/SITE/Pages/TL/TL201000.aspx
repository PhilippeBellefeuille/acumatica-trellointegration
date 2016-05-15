<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TL201000.aspx.cs" Inherits="Page_TL201000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" TypeName="PX.TrelloIntegration.TrelloBoardSetup" PrimaryView="Board" SuspendUnloading="False">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="Board" TabIndex="2300">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True"/>
		    <px:PXSelector ID="edTrelloBoardID" runat="server" DataField="TrelloBoardID">
            </px:PXSelector>
            <px:PXSelector ID="edCaseClassID" runat="server" DataField="CaseClassID">
            </px:PXSelector>
            <px:PXTextEdit ID="edName" runat="server" DataField="Name">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edDescr" runat="server" DataField="Descr">
            </px:PXTextEdit>
            <px:PXTextEdit ID="edUrlDescr" runat="server" DataField="UrlDescr">
            </px:PXTextEdit>
            <px:PXCheckBox ID="edActive" runat="server" DataField="Active" Text="Active">
            </px:PXCheckBox>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
<%--	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="Details">
		<Levels>
			<px:PXGridLevel>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXGrid>--%>
</asp:Content>
