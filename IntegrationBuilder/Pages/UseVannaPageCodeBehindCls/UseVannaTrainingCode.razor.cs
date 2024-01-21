using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using IntegrationBuilder;
using IntegrationBuilder.Shared;
using Radzen;
using Radzen.Blazor;
using IntegrationBuilder.SQLServerUtilities;
using IntegrationBuilder.VannaAIUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IntegrationBuilder.Pages
{
    public partial class UseVanna
    {
        //All code related to Model Training
        //Parameters
        private List<string> _allTables;
        private List<string> _allViews;
        private List<DDLDocumentation> _tableDDLDocumentations;
        private List<DDLDocumentation> _viewDDLDocumentations;
        private string _sqlStatementForTrain;
        private string _documentationForTrain;
        RadzenDataGrid<DDLDocumentation> _tableDDLDocumentationsGrid;
        RadzenDataGrid<DDLDocumentation> _viewDDLDocumentationsGrid;
        DDLDocumentation _tableDocumentationToInsert;
        DDLDocumentation _tableDocumentationToUpdate;
        DDLDocumentation _tableDocumentationSafe;
        DDLDocumentation _viewDocumentationToInsert;
        DDLDocumentation _viewDocumentationToUpdate;
        DDLDocumentation _viewDocumentationSafe;
        //---------------------------------
        //Functions
        //---------------------------------
        private bool GetAllInitialDataForStep3()
        {
            try
            {
                StringBuilder urlBuilder = new StringBuilder();
                urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                urlBuilder.Append("/");
                urlBuilder.Append(configuration["ApplicationInfo:getAllTableNames"].ToString());
                string error = "";
                this._allTables = _vannaUtilitiesService.GetInfosFromDB(urlBuilder.ToString(), this._sqlServerCredentials, out error);
                if (this._allTables == null)
                {
                    this._infomsgs = $"GetInfosFromDB returns null. Error message: {error}";
                    return false;
                }

                urlBuilder = new StringBuilder();
                urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                urlBuilder.Append("/");
                urlBuilder.Append(configuration["ApplicationInfo:getAllViewNames"].ToString());
                this._allViews = _vannaUtilitiesService.GetInfosFromDB(urlBuilder.ToString(), this._sqlServerCredentials, out error);
                if (this._allViews == null)
                {
                    this._infomsgs = $"GetInfosFromDB returns null. Error message: {error}";
                    return false;
                }

                DDLDocumentation d = new DDLDocumentation();
                return true;
            }
            catch (Exception exc)
            {
                this._infomsgs = $"Exception in GetAllInitialData. Exception message: {exc.Message}";
                return false;
            }
        }

        void ResetTableGenVars()
        {
            _tableDocumentationToInsert = null;
            _tableDocumentationToUpdate = null;
        }

        void ResetViewGenVars()
        {
            _viewDocumentationToInsert = null;
            _viewDocumentationToUpdate = null;
        }

        async Task EditRowTables(DDLDocumentation tdoc)
        {
            this._tableDocumentationSafe = new DDLDocumentation();
            _tableDocumentationSafe.documentation = tdoc.documentation;
            _tableDocumentationSafe.name = tdoc.name;
            this._tableDocumentationToUpdate = tdoc;
            await this._tableDDLDocumentationsGrid.EditRow(this._tableDocumentationToUpdate);
        //ResetTableGenVars();
        }

        async Task EditRowViews(DDLDocumentation tdoc)
        {
            this._viewDocumentationSafe = new DDLDocumentation();
            _viewDocumentationSafe.documentation = tdoc.documentation;
            _viewDocumentationSafe.name = tdoc.name;
            this._viewDocumentationToUpdate = tdoc;
            await this._viewDDLDocumentationsGrid.EditRow(this._viewDocumentationToUpdate);
        //ResetViewGenVars();
        }

        void OnUpdateRowTables(DDLDocumentation tdoc)
        {
            this._tableDocumentationToUpdate = tdoc;
            this._tableDDLDocumentationsGrid.Reload();
            ResetTableGenVars();
        }

        void OnUpdateRowViews(DDLDocumentation tdoc)
        {
            this._viewDocumentationToUpdate = tdoc;
            this._viewDDLDocumentationsGrid.Reload();
            ResetViewGenVars();
        }

        async Task SaveRowTables(DDLDocumentation tdoc)
        {
            if (string.IsNullOrEmpty(tdoc.name))
            {
                this._infomsgs = "Select Table to insert row!";
                return;
            }

            if (string.IsNullOrEmpty(tdoc.documentation))
            {
                this._infomsgs = "Add documentation to insert row!";
                return;
            }

            await this._tableDDLDocumentationsGrid.UpdateRow(tdoc);
        }

        async Task SaveRowViews(DDLDocumentation tdoc)
        {
            if (string.IsNullOrEmpty(tdoc.name))
            {
                this._infomsgs = "Select View to insert row!";
                return;
            }

            if (string.IsNullOrEmpty(tdoc.documentation))
            {
                this._infomsgs = "Add documentation to insert row!";
                return;
            }

            await this._viewDDLDocumentationsGrid.UpdateRow(tdoc);
        }

        void CancelEditTables(DDLDocumentation tdoc)
        {
            if (this._tableDDLDocumentations.Any(i => i.name.Equals(tdoc.name) && i.documentation.Equals(tdoc.documentation)))
            {
                int index = this._tableDDLDocumentations.FindIndex(i => i.name.Equals(tdoc.name) && i.documentation.Equals(tdoc.documentation));
                if (index != -1)
                    this._tableDDLDocumentations[index] = this._tableDocumentationSafe;
                ResetTableGenVars();
                this._tableDDLDocumentationsGrid.CancelEditRow(tdoc);
                this._tableDDLDocumentationsGrid.Reload();
            }
            else
            {
                ResetTableGenVars();
                this._tableDDLDocumentationsGrid.CancelEditRow(tdoc);
                this._tableDDLDocumentationsGrid.Reload();
            }
        }

        void CancelEditViews(DDLDocumentation tdoc)
        {
            if (this._viewDDLDocumentations.Any(i => i.name.Equals(tdoc.name) && i.documentation.Equals(tdoc.documentation)))
            {
                int index = this._viewDDLDocumentations.FindIndex(i => i.name.Equals(tdoc.name) && i.documentation.Equals(tdoc.documentation));
                if (index != -1)
                    this._viewDDLDocumentations[index] = this._viewDocumentationSafe;
                ResetViewGenVars();
                this._viewDDLDocumentationsGrid.CancelEditRow(tdoc);
                this._viewDDLDocumentationsGrid.Reload();
            }
            else
            {
                ResetViewGenVars();
                this._viewDDLDocumentationsGrid.CancelEditRow(tdoc);
                this._viewDDLDocumentationsGrid.Reload();
            }
        }

        async Task DeleteRowTables(DDLDocumentation tdoc)
        {
            ResetTableGenVars();
            if (this._tableDDLDocumentations.Contains(tdoc))
            {
                this._tableDDLDocumentations.Remove(tdoc);
                await this._tableDDLDocumentationsGrid.Reload();
            }
            else
            {
                this._tableDDLDocumentationsGrid.CancelEditRow(tdoc);
                await this._tableDDLDocumentationsGrid.Reload();
            }
        }

        async Task DeleteRowViews(DDLDocumentation tdoc)
        {
            ResetViewGenVars();
            if (this._viewDDLDocumentations.Contains(tdoc))
            {
                this._viewDDLDocumentations.Remove(tdoc);
                await this._viewDDLDocumentationsGrid.Reload();
            }
            else
            {
                this._viewDDLDocumentationsGrid.CancelEditRow(tdoc);
                await this._viewDDLDocumentationsGrid.Reload();
            }
        }

        async Task BtnTrainWithTables()
        {
            try
            {
                this._isLoadingTrainWithTables = true;
                this.StateHasChanged();
                // InvokeAsync(StateHasChanged);
                this._infomsgs = "";
                if (this._tableDDLDocumentations == null)
                {
                    this._infomsgs = "_tableDDLDocumentations is null";
                    return;
                }

                await Task.Run(async () =>
                {
                    List<string> tableNames = new List<string>();
                    List<string> documentation = new List<string>();
                    foreach (DDLDocumentation d in this._tableDDLDocumentations)
                    {
                        tableNames.Add(d.name);
                        documentation.Add(d.documentation);
                    }

                    StringBuilder urlBuilder = new StringBuilder();
                    urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                    urlBuilder.Append("/");
                    urlBuilder.Append(configuration["ApplicationInfo:trainWithTables"].ToString());
                    string errorMsg = "";
                    if (this._vannaUtilitiesService.TrainModelWithTables(urlBuilder.ToString(), tableNames.Distinct().ToList(), this._vannaModel.ModelName.ToLower(), this._sqlServerCredentials, out errorMsg))
                    {
                        this._infomsgs = "Train with tables OK. Now will start the training with Documentation!";
                    }
                    else
                    {
                        this._infomsgs = $"Something went wrong in Train with tables. Message: {errorMsg}";
                        this._isLoadingTrainWithTables = false;
                        this.StateHasChanged();
                        return;
                    }

                    await TrainWithDocumentation(documentation);
                });
                this._isLoadingTrainWithTables = false;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                this._infomsgs = $"Exception in BtnTrainWithTables. Exception message: {exc.Message}";
                this._isLoadingTrainWithTables = false;
                this.StateHasChanged();
                return;
            }
        }

        async Task BtnTrainWithDocumentation()
        {
            try
            {
                this._infomsgs = "";
                this._isLoadingTrainWithDocumentation = true;
                this.StateHasChanged();
                await Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(this._documentationForTrain))
                    {
                        this._infomsgs = "Add some documentation!";
                        return;
                    }

                    List<string> tmp = new List<string>();
                    tmp.Add(this._documentationForTrain);
                    TrainWithDocumentation(tmp);
                });
                this._isLoadingTrainWithDocumentation = false;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                this._infomsgs = $"Exception in BtnTrainWithDocumentation. Exception message: {exc.Message}";
                this._isLoadingTrainWithDocumentation = false;
                this.StateHasChanged();
            }
        }

        async Task BtnTrainWithViews()
        {
            try
            {
                this._infomsgs = "";
                if (this._viewDDLDocumentations == null)
                {
                    this._infomsgs = "_viewDDLDocumentations is null";
                    return;
                }

                this._isLoadingTrainWithViews = true;
                this.StateHasChanged();
                await Task.Run(async () =>
                {
                    List<string> viewNames = new List<string>();
                    List<string> documentation = new List<string>();
                    foreach (DDLDocumentation d in this._viewDDLDocumentations)
                    {
                        viewNames.Add(d.name);
                        documentation.Add(d.documentation);
                    }

                    StringBuilder urlBuilder = new StringBuilder();
                    urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                    urlBuilder.Append("/");
                    urlBuilder.Append(configuration["ApplicationInfo:trainWithViews"].ToString());
                    string errorMsg = "";
                    if (this._vannaUtilitiesService.TrainModelWithViews(urlBuilder.ToString(), viewNames.Distinct().ToList(), this._vannaModel.ModelName.ToLower(), this._sqlServerCredentials, out errorMsg))
                    {
                        this._infomsgs = "Train with views OK. Now will start the training with Documentation!";
                    }
                    else
                    {
                        this._infomsgs = $"Something went wrong in Train with views. Message: {errorMsg}";
                        this._isLoadingTrainWithViews = false;
                        this.StateHasChanged();
                        return;
                    }

                    TrainWithDocumentation(documentation);
                });
                this._isLoadingTrainWithViews = false;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                this._infomsgs = $"Exception in BtnTrainWithViews. Exception message: {exc.Message}";
                this._isLoadingTrainWithViews = false;
                this.StateHasChanged();
                return;
            }
        }

        async Task BtnTrainWithSQL()
        {
            try
            {
                this._infomsgs = "";
                if (string.IsNullOrEmpty(this._sqlStatementForTrain))
                {
                    this._infomsgs = "SQL field is empty!";
                    return;
                }

                this._isLoadingTrainWithSql = true;
                this.StateHasChanged();
                await Task.Run(async () =>
                {
                    StringBuilder urlBuilder = new StringBuilder();
                    urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                    urlBuilder.Append("/");
                    urlBuilder.Append(configuration["ApplicationInfo:trainWithSQL"].ToString());
                    string error = "";
                    if (!this._vannaUtilitiesService.TrainWithDocumentation(urlBuilder.ToString(), this._sqlStatementForTrain.Trim(), this._vannaModel.ModelName.ToLower(), out error))
                    {
                        this._infomsgs += "\n" + $"Problem in BtnTrainWithSQL {this._sqlStatementForTrain.Trim()}.\n Exception message: {error}";
                    }
                    else
                    {
                        this._infomsgs += $"Training with SQL Ok";
                    }
                });
                this._isLoadingTrainWithSql = false;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                _infomsgs = $"Exception in BtnTrainWithSQL. Exception message: {exc.Message}";
                this._isLoadingTrainWithSql = false;
                this.StateHasChanged();
            }
        }

        async Task TrainWithDocumentation(List<string> documentation)
        {
            try
            {
                this._infomsgs = "";
                StringBuilder urlBuilder = new StringBuilder();
                urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                urlBuilder.Append("/");
                urlBuilder.Append(configuration["ApplicationInfo:trainWithDocumentation"].ToString());
                foreach (string doc in documentation)
                {
                    string error = "";
                    if (!this._vannaUtilitiesService.TrainWithDocumentation(urlBuilder.ToString(), doc, this._vannaModel.ModelName.ToLower(), out error))
                    {
                        this._infomsgs += "\n" + $"Problem in train with documentation {doc}.\n Exception message: {error}";
                        return;
                    }
                }

                this._infomsgs += "\n" + "Train with Documentation Ok";
            }
            catch (Exception exc)
            {
                this._infomsgs += "\n" + $"Exception in TrainWithDocumentation. Exception message:{exc.Message}";
            }
        }

        async Task InsertRowTable()
        {
            this._tableDocumentationToInsert = new DDLDocumentation();
            await this._tableDDLDocumentationsGrid.InsertRow(this._tableDocumentationToInsert);
        }

        async Task InsertRowView()
        {
            this._viewDocumentationToInsert = new DDLDocumentation();
            await this._viewDDLDocumentationsGrid.InsertRow(this._viewDocumentationToInsert);
        }

        void OnCreateRowTables(DDLDocumentation tdoc)
        {
            this._tableDocumentationToInsert = tdoc;
            this._tableDDLDocumentations.Add(this._tableDocumentationToInsert);
            ResetTableGenVars();
            this._tableDDLDocumentationsGrid.Reload();
        }

        void OnCreateRowViews(DDLDocumentation tdoc)
        {
            this._viewDocumentationToInsert = tdoc;
            this._viewDDLDocumentations.Add(this._viewDocumentationToInsert);
            ResetViewGenVars();
            this._viewDDLDocumentationsGrid.Reload();
        }
    }
}