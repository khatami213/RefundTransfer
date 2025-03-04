﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace RefundTransferService.WebServiceRefund {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RefundSoap", Namespace="http://tempuri.org/")]
    public partial class Refund : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback NormalTransferOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLastTransactionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Refund() {
            this.Url = global::RefundTransferService.Properties.Settings.Default.RefundTransferService_WebServiceRefund_Refund;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event NormalTransferCompletedEventHandler NormalTransferCompleted;
        
        /// <remarks/>
        public event GetLastTransactionCompletedEventHandler GetLastTransactionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/NormalTransfer", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string NormalTransfer(string SourceDepositNo, string DestinationDepositNo, decimal Amount, string DestinationComment, string SourceComment, int YaghutTimeOut, string User, string Pass) {
            object[] results = this.Invoke("NormalTransfer", new object[] {
                        SourceDepositNo,
                        DestinationDepositNo,
                        Amount,
                        DestinationComment,
                        SourceComment,
                        YaghutTimeOut,
                        User,
                        Pass});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void NormalTransferAsync(string SourceDepositNo, string DestinationDepositNo, decimal Amount, string DestinationComment, string SourceComment, int YaghutTimeOut, string User, string Pass) {
            this.NormalTransferAsync(SourceDepositNo, DestinationDepositNo, Amount, DestinationComment, SourceComment, YaghutTimeOut, User, Pass, null);
        }
        
        /// <remarks/>
        public void NormalTransferAsync(string SourceDepositNo, string DestinationDepositNo, decimal Amount, string DestinationComment, string SourceComment, int YaghutTimeOut, string User, string Pass, object userState) {
            if ((this.NormalTransferOperationCompleted == null)) {
                this.NormalTransferOperationCompleted = new System.Threading.SendOrPostCallback(this.OnNormalTransferOperationCompleted);
            }
            this.InvokeAsync("NormalTransfer", new object[] {
                        SourceDepositNo,
                        DestinationDepositNo,
                        Amount,
                        DestinationComment,
                        SourceComment,
                        YaghutTimeOut,
                        User,
                        Pass}, this.NormalTransferOperationCompleted, userState);
        }
        
        private void OnNormalTransferOperationCompleted(object arg) {
            if ((this.NormalTransferCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.NormalTransferCompleted(this, new NormalTransferCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetLastTransaction", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public statementResponseBean GetLastTransaction(string DepositNo, string user, string pass) {
            object[] results = this.Invoke("GetLastTransaction", new object[] {
                        DepositNo,
                        user,
                        pass});
            return ((statementResponseBean)(results[0]));
        }
        
        /// <remarks/>
        public void GetLastTransactionAsync(string DepositNo, string user, string pass) {
            this.GetLastTransactionAsync(DepositNo, user, pass, null);
        }
        
        /// <remarks/>
        public void GetLastTransactionAsync(string DepositNo, string user, string pass, object userState) {
            if ((this.GetLastTransactionOperationCompleted == null)) {
                this.GetLastTransactionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLastTransactionOperationCompleted);
            }
            this.InvokeAsync("GetLastTransaction", new object[] {
                        DepositNo,
                        user,
                        pass}, this.GetLastTransactionOperationCompleted, userState);
        }
        
        private void OnGetLastTransactionOperationCompleted(object arg) {
            if ((this.GetLastTransactionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLastTransactionCompleted(this, new GetLastTransactionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.yaghut.modern.tosan.com/")]
    public partial class statementResponseBean {
        
        private statementBean[] statementBeansField;
        
        private long totalRecordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("statementBeans", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public statementBean[] statementBeans {
            get {
                return this.statementBeansField;
            }
            set {
                this.statementBeansField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long totalRecord {
            get {
                return this.totalRecordField;
            }
            set {
                this.totalRecordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.yaghut.modern.tosan.com/")]
    public partial class statementBean {
        
        private string agentBranchCodeField;
        
        private string agentBranchNameField;
        
        private decimal balanceField;
        
        private bool balanceFieldSpecified;
        
        private string branchCodeField;
        
        private string branchNameField;
        
        private System.DateTime dateField;
        
        private bool dateFieldSpecified;
        
        private string descriptionField;
        
        private string paymentIdField;
        
        private string referenceNumberField;
        
        private long registrationNumberField;
        
        private bool registrationNumberFieldSpecified;
        
        private long serialField;
        
        private bool serialFieldSpecified;
        
        private string serialNumberField;
        
        private string statementIdField;
        
        private decimal transferAmountField;
        
        private bool transferAmountFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agentBranchCode {
            get {
                return this.agentBranchCodeField;
            }
            set {
                this.agentBranchCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agentBranchName {
            get {
                return this.agentBranchNameField;
            }
            set {
                this.agentBranchNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal balance {
            get {
                return this.balanceField;
            }
            set {
                this.balanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool balanceSpecified {
            get {
                return this.balanceFieldSpecified;
            }
            set {
                this.balanceFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string branchCode {
            get {
                return this.branchCodeField;
            }
            set {
                this.branchCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string branchName {
            get {
                return this.branchNameField;
            }
            set {
                this.branchNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateSpecified {
            get {
                return this.dateFieldSpecified;
            }
            set {
                this.dateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentId {
            get {
                return this.paymentIdField;
            }
            set {
                this.paymentIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string referenceNumber {
            get {
                return this.referenceNumberField;
            }
            set {
                this.referenceNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long registrationNumber {
            get {
                return this.registrationNumberField;
            }
            set {
                this.registrationNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool registrationNumberSpecified {
            get {
                return this.registrationNumberFieldSpecified;
            }
            set {
                this.registrationNumberFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long serial {
            get {
                return this.serialField;
            }
            set {
                this.serialField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool serialSpecified {
            get {
                return this.serialFieldSpecified;
            }
            set {
                this.serialFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serialNumber {
            get {
                return this.serialNumberField;
            }
            set {
                this.serialNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statementId {
            get {
                return this.statementIdField;
            }
            set {
                this.statementIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal transferAmount {
            get {
                return this.transferAmountField;
            }
            set {
                this.transferAmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transferAmountSpecified {
            get {
                return this.transferAmountFieldSpecified;
            }
            set {
                this.transferAmountFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void NormalTransferCompletedEventHandler(object sender, NormalTransferCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class NormalTransferCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal NormalTransferCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void GetLastTransactionCompletedEventHandler(object sender, GetLastTransactionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLastTransactionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLastTransactionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public statementResponseBean Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((statementResponseBean)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591