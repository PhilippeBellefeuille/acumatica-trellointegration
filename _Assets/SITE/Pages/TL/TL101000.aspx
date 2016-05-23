<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TL101000.aspx.cs" Inherits="Page_TL101000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" PrimaryView="Document" SuspendUnloading="False" TypeName="PX.TrelloIntegration.TrelloSetupMaint">
	    <CallbackCommands>
            <px:PXDSCallbackCommand CommitChanges="True" Name="Login" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="CompleteAuthentication" Visible="False" />
        </CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeView="Boards" TreeKeys="BoardID" />
        </DataTrees>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document" TabIndex="3900">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True"/>
		    <px:PXDateTimeEdit runat="server" DataField="ConnectionDateTime" ID="edConnectionDateTime" Enabled="False" />
            <px:PXLayoutRule runat="server" StartColumn="True"/>
            <px:PXTextEdit ID="edUserName" runat="server" DataField="UserName" Enabled="False"/>
            <px:PXLayoutRule runat="server" StartRow="True" ColumnSpan="2"/>
            <px:PXSelector ID="edTrelloOrganizationID" runat="server" DataField="TrelloOrganizationID" />
		</Template>
	</px:PXFormView>    
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
    <px:PXSplitContainer runat="server" ID="sp1" SplitterPosition="300">
        <AutoSize Enabled="true" Container="Window" />
        <Template1>
            <px:PXTreeView ID="tree" runat="server" DataSourceID="ds" Height="180px"
                ShowRootNode="False" AllowCollapse="False" Caption="Boards Mapping" AutoRepaint="True"
                SyncPosition="True" ExpandDepth="4" DataMember="Boards" KeepPosition="True" 
                SyncPositionWithGraph="True" PreserveExpanded="True" PopulateOnDemand="true" SelectFirstNode="True">
                <ToolBarItems>
                    <px:PXToolBarButton Text="Add Board Binding" Tooltip="Add Board Binding">
                        <AutoCallBack Command="AddBoard" Enabled="True" Target="ds" />
                        <Images Normal="main@AddNew" />
                    </px:PXToolBarButton>
                    
                    <px:PXToolBarButton Text="Delete Board Binding" Tooltip="Delete Board Binding">
                        <AutoCallBack Command="DeleteBoard" Enabled="True" Target="ds" />
                        <Images Normal="main@Remove" />
                    </px:PXToolBarButton>
                </ToolBarItems>
                <AutoCallBack Target="formBoard" Command="Refresh" Enabled="True" />
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="Boards" TextField="DisplayName" ValueField="BoardID" />
                </DataBindings>
                <AutoSize Enabled="True" />
            </px:PXTreeView>
        </Template1>
        <Template2>
            <px:PXFormView ID="formBoard" runat="server" DataSourceID="ds" DataMember="CurrentBoard" 
                        Caption="Board Info" Width="100%" >
                <Template>
                    <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartColumn="True" LabelsWidth="S" ControlSize="SM" />
                    <px:PXDropDown ID="edBoardType" runat="server" DataField="BoardType" CommitChanges="true" />
                    <px:PXSelector ID="edCaseClassID" runat="server" DataField="CaseClassID" CommitChanges="True"/>                    
                    <px:PXLayoutRule ID="PXLayoutRule2" runat="server" StartColumn="True" LabelsWidth="S" ControlSize="SM" />
                    <px:PXSelector ID="edTrelloBoardID" runat="server" DataField="TrelloBoardID" CommitChanges="True"/>
                </Template>
            </px:PXFormView>
            <px:PXTab runat="server" ID="tab" >
                <Items>
                    <px:PXTabItem Text="States" LoadOnDemand="True" RepaintOnDemand="True">
                        <Template>
                            <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		                        Width="100%" SkinID="Inquire" TabIndex="1500">
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
		                        <AutoSize Container="Window" Enabled="True" />
	                        </px:PXGrid>
                        </Template>
                    </px:PXTabItem>
                    <px:PXTabItem Text="Users" LoadOnDemand="True" RepaintOnDemand="True">
                        <Template>

                        </Template>
                    </px:PXTabItem>
                </Items>
            </px:PXTab>
        </Template2>
    </px:PXSplitContainer>

</asp:Content>
