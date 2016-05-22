<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TL201000.aspx.cs" Inherits="Page_TL201000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" TypeName="PX.TrelloIntegration.TrelloBoardMappingMaint" PrimaryView="Board" SuspendUnloading="False">
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
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="Details" TabIndex="1500">
		<Levels>
			<px:PXGridLevel DataKeyNames="BoardID,ListID" DataMember="List">
			    <RowTemplate>
                    <px:PXSelector ID="edStepID" runat="server" DataField="StepID">
                    </px:PXSelector>
                    <px:PXSelector ID="edTrelloListID" runat="server" DataField="TrelloListID">
                    </px:PXSelector>
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="StepID" Width="200px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="TrelloListID" Width="120px">
                    </px:PXGridColumn>
                </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXGrid>
</asp:Content>
