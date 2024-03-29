﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Этот исходный текст был создан автоматически: Microsoft.VSDesigner, версия: 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace SendTaskTo1C.WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WebСервис_Приемка_АРЕНАSoapBinding", Namespace="http://localhost/")]
    public partial class WebСервис_Приемка_АРЕНА : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LoadReceiptsOperationCompleted;
        
        private System.Threading.SendOrPostCallback LoadReceipts_newOperationCompleted;
        
        private System.Threading.SendOrPostCallback LoadOrderToSuppliersOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WebСервис_Приемка_АРЕНА() {
            this.Url = global::SendTaskTo1C.Properties.Settings.Default.SendTaskTo1C_WebReference_WebСервис_Приемка_АРЕНА;
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
        public event LoadReceiptsCompletedEventHandler LoadReceiptsCompleted;
        
        /// <remarks/>
        public event LoadReceipts_newCompletedEventHandler LoadReceipts_newCompleted;
        
        /// <remarks/>
        public event LoadOrderToSuppliersCompletedEventHandler LoadOrderToSuppliersCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/#WebСервис_Приемка_АРЕНА:LoadReceipts", RequestNamespace="http://localhost/", ResponseNamespace="http://localhost/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("return", IsNullable=true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("Param", Namespace="http://www.Receipts", IsNullable=false)]
        public Error[] LoadReceipts([System.Xml.Serialization.XmlArrayItemAttribute("Param", Namespace="http://www.Receipts", IsNullable=false)] Receipt[] Receipts) {
            object[] results = this.Invoke("LoadReceipts", new object[] {
                        Receipts});
            return ((Error[])(results[0]));
        }
        
        /// <remarks/>
        public void LoadReceiptsAsync(Receipt[] Receipts) {
            this.LoadReceiptsAsync(Receipts, null);
        }
        
        /// <remarks/>
        public void LoadReceiptsAsync(Receipt[] Receipts, object userState) {
            if ((this.LoadReceiptsOperationCompleted == null)) {
                this.LoadReceiptsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadReceiptsOperationCompleted);
            }
            this.InvokeAsync("LoadReceipts", new object[] {
                        Receipts}, this.LoadReceiptsOperationCompleted, userState);
        }
        
        private void OnLoadReceiptsOperationCompleted(object arg) {
            if ((this.LoadReceiptsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadReceiptsCompleted(this, new LoadReceiptsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/#WebСервис_Приемка_АРЕНА:LoadReceipts_new", RequestNamespace="http://localhost/", ResponseNamespace="http://localhost/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("return", IsNullable=true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("Param", Namespace="http://www.Receipts", IsNullable=false)]
        public Error[] LoadReceipts_new([System.Xml.Serialization.XmlArrayItemAttribute("Param", Namespace="http://www.Receipts", IsNullable=false)] Receipt[] Receipts) {
            object[] results = this.Invoke("LoadReceipts_new", new object[] {
                        Receipts});
            return ((Error[])(results[0]));
        }
        
        /// <remarks/>
        public void LoadReceipts_newAsync(Receipt[] Receipts) {
            this.LoadReceipts_newAsync(Receipts, null);
        }
        
        /// <remarks/>
        public void LoadReceipts_newAsync(Receipt[] Receipts, object userState) {
            if ((this.LoadReceipts_newOperationCompleted == null)) {
                this.LoadReceipts_newOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadReceipts_newOperationCompleted);
            }
            this.InvokeAsync("LoadReceipts_new", new object[] {
                        Receipts}, this.LoadReceipts_newOperationCompleted, userState);
        }
        
        private void OnLoadReceipts_newOperationCompleted(object arg) {
            if ((this.LoadReceipts_newCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadReceipts_newCompleted(this, new LoadReceipts_newCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/#WebСервис_Приемка_АРЕНА:LoadOrderToSuppliers", RequestNamespace="http://localhost/", ResponseNamespace="http://localhost/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", IsNullable=true)]
        public ResultOrderToSupplier LoadOrderToSuppliers(string PlanWMSNumber) {
            object[] results = this.Invoke("LoadOrderToSuppliers", new object[] {
                        PlanWMSNumber});
            return ((ResultOrderToSupplier)(results[0]));
        }
        
        /// <remarks/>
        public void LoadOrderToSuppliersAsync(string PlanWMSNumber) {
            this.LoadOrderToSuppliersAsync(PlanWMSNumber, null);
        }
        
        /// <remarks/>
        public void LoadOrderToSuppliersAsync(string PlanWMSNumber, object userState) {
            if ((this.LoadOrderToSuppliersOperationCompleted == null)) {
                this.LoadOrderToSuppliersOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoadOrderToSuppliersOperationCompleted);
            }
            this.InvokeAsync("LoadOrderToSuppliers", new object[] {
                        PlanWMSNumber}, this.LoadOrderToSuppliersOperationCompleted, userState);
        }
        
        private void OnLoadOrderToSuppliersOperationCompleted(object arg) {
            if ((this.LoadOrderToSuppliersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoadOrderToSuppliersCompleted(this, new LoadOrderToSuppliersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class Receipt {
        
        private string gUID_WEBField;
        
        private string typeDocField;
        
        private System.DateTime dateDocField;
        
        private string numberDocField;
        
        private string gUID_LocationField;
        
        private string gUID_DivisionField;
        
        private ReceiptGood[] receiptGoodField;
        
        private System.DateTime dateReceiptField;
        
        private System.DateTime dateBeginLoadField;
        
        private System.DateTime dateEndLoadField;
        
        private string rowpicturesField;
        
        /// <remarks/>
        public string GUID_WEB {
            get {
                return this.gUID_WEBField;
            }
            set {
                this.gUID_WEBField = value;
            }
        }
        
        /// <remarks/>
        public string TypeDoc {
            get {
                return this.typeDocField;
            }
            set {
                this.typeDocField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DateDoc {
            get {
                return this.dateDocField;
            }
            set {
                this.dateDocField = value;
            }
        }
        
        /// <remarks/>
        public string NumberDoc {
            get {
                return this.numberDocField;
            }
            set {
                this.numberDocField = value;
            }
        }
        
        /// <remarks/>
        public string GUID_Location {
            get {
                return this.gUID_LocationField;
            }
            set {
                this.gUID_LocationField = value;
            }
        }
        
        /// <remarks/>
        public string GUID_Division {
            get {
                return this.gUID_DivisionField;
            }
            set {
                this.gUID_DivisionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ReceiptGood")]
        public ReceiptGood[] ReceiptGood {
            get {
                return this.receiptGoodField;
            }
            set {
                this.receiptGoodField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DateReceipt {
            get {
                return this.dateReceiptField;
            }
            set {
                this.dateReceiptField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DateBeginLoad {
            get {
                return this.dateBeginLoadField;
            }
            set {
                this.dateBeginLoadField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DateEndLoad {
            get {
                return this.dateEndLoadField;
            }
            set {
                this.dateEndLoadField = value;
            }
        }
        
        /// <remarks/>
        public string Rowpictures {
            get {
                return this.rowpicturesField;
            }
            set {
                this.rowpicturesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class ReceiptGood {
        
        private string articleField;
        
        private decimal quantityField;
        
        private string barcodeField;
        
        private string goodBarcodeField;
        
        /// <remarks/>
        public string Article {
            get {
                return this.articleField;
            }
            set {
                this.articleField = value;
            }
        }
        
        /// <remarks/>
        public decimal Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Barcode {
            get {
                return this.barcodeField;
            }
            set {
                this.barcodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GoodBarcode {
            get {
                return this.goodBarcodeField;
            }
            set {
                this.goodBarcodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class OrderToSupplierGood {
        
        private string articleField;
        
        private System.Nullable<decimal> quantityField;
        
        private string barcodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Article {
            get {
                return this.articleField;
            }
            set {
                this.articleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Barcode {
            get {
                return this.barcodeField;
            }
            set {
                this.barcodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class OrderToSupplier {
        
        private string gUID_PlanWMSNumberField;
        
        private string typeDocField;
        
        private System.Nullable<System.DateTime> dateDocField;
        
        private string numberDocField;
        
        private OrderToSupplierGood[] orderToSupplierGoodField;
        
        private string gUID_LocationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GUID_PlanWMSNumber {
            get {
                return this.gUID_PlanWMSNumberField;
            }
            set {
                this.gUID_PlanWMSNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string TypeDoc {
            get {
                return this.typeDocField;
            }
            set {
                this.typeDocField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> DateDoc {
            get {
                return this.dateDocField;
            }
            set {
                this.dateDocField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string NumberDoc {
            get {
                return this.numberDocField;
            }
            set {
                this.numberDocField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OrderToSupplierGood", IsNullable=true)]
        public OrderToSupplierGood[] OrderToSupplierGood {
            get {
                return this.orderToSupplierGoodField;
            }
            set {
                this.orderToSupplierGoodField = value;
            }
        }
        
        /// <remarks/>
        public string GUID_Location {
            get {
                return this.gUID_LocationField;
            }
            set {
                this.gUID_LocationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class ResultOrderToSupplier {
        
        private OrderToSupplier[] paramField;
        
        private Error[] errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Param", IsNullable=true)]
        public OrderToSupplier[] Param {
            get {
                return this.paramField;
            }
            set {
                this.paramField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Error", IsNullable=true)]
        public Error[] Error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class Error {
        
        private Message[] messagesField;
        
        private string gUID_WEBField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Messages")]
        public Message[] Messages {
            get {
                return this.messagesField;
            }
            set {
                this.messagesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GUID_WEB {
            get {
                return this.gUID_WEBField;
            }
            set {
                this.gUID_WEBField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.Receipts")]
    public partial class Message {
        
        private string codeField;
        
        private string message1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Message")]
        public string Message1 {
            get {
                return this.message1Field;
            }
            set {
                this.message1Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void LoadReceiptsCompletedEventHandler(object sender, LoadReceiptsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadReceiptsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoadReceiptsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Error[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Error[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void LoadReceipts_newCompletedEventHandler(object sender, LoadReceipts_newCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadReceipts_newCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoadReceipts_newCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Error[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Error[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void LoadOrderToSuppliersCompletedEventHandler(object sender, LoadOrderToSuppliersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoadOrderToSuppliersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoadOrderToSuppliersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResultOrderToSupplier Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResultOrderToSupplier)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591