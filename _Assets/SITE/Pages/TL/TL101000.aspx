<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TL101000.aspx.cs" Inherits="Page_TL101000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" PrimaryView="TrelloSetup" SuspendUnloading="False" TypeName="PX.TrelloIntegration.TrelloSetupMaint">
	    <CallbackCommands>
            <px:PXDSCallbackCommand Name="Cancel" Visible="False" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Login" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="CompleteAuthentication" Visible="False" />
        </CallbackCommands>
    </px:PXDataSource>
    
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="TrelloSetup" TabIndex="3900">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True"/>
		    <px:PXTextEdit ID="edTrelloUsrToken" runat="server" DataField="TrelloUsrToken">
            </px:PXTextEdit>
            <px:PXSelector ID="edTrelloOrganizationID" runat="server" DataField="TrelloOrganizationID">
            </px:PXSelector>
		</Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXFormView>
    
</asp:Content>
