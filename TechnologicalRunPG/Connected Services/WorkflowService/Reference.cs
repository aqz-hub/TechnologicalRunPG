//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechnologicalRunPG.WorkflowService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebData", Namespace="http://schemas.datacontract.org/2004/07/EleWise.ELMA.Common.Models")]
    [System.SerializableAttribute()]
    public partial class WebData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TechnologicalRunPG.WorkflowService.WebDataItem[] ItemsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TechnologicalRunPG.WorkflowService.WebDataItem[] Items {
            get {
                return this.ItemsField;
            }
            set {
                if ((object.ReferenceEquals(this.ItemsField, value) != true)) {
                    this.ItemsField = value;
                    this.RaisePropertyChanged("Items");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebDataItem", Namespace="http://schemas.datacontract.org/2004/07/EleWise.ELMA.Common.Models")]
    [System.SerializableAttribute()]
    public partial class WebDataItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TechnologicalRunPG.WorkflowService.WebData DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TechnologicalRunPG.WorkflowService.WebData[] DataArrayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TechnologicalRunPG.WorkflowService.WebData Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TechnologicalRunPG.WorkflowService.WebData[] DataArray {
            get {
                return this.DataArrayField;
            }
            set {
                if ((object.ReferenceEquals(this.DataArrayField, value) != true)) {
                    this.DataArrayField = value;
                    this.RaisePropertyChanged("DataArray");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PublicServiceException", Namespace="http://schemas.datacontract.org/2004/07/EleWise.ELMA.Services.Public")]
    [System.SerializableAttribute()]
    public partial class PublicServiceException : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TechnologicalRunPG.WorkflowService.PublicServiceException InnerExceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int StatusCodeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TechnologicalRunPG.WorkflowService.PublicServiceException InnerException {
            get {
                return this.InnerExceptionField;
            }
            set {
                if ((object.ReferenceEquals(this.InnerExceptionField, value) != true)) {
                    this.InnerExceptionField = value;
                    this.RaisePropertyChanged("InnerException");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StatusCode {
            get {
                return this.StatusCodeField;
            }
            set {
                if ((this.StatusCodeField.Equals(value) != true)) {
                    this.StatusCodeField = value;
                    this.RaisePropertyChanged("StatusCode");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.elma-bpm.ru/api/Workflow", ConfigurationName="WorkflowService.Workflow")]
    public interface Workflow {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/TaskStandardOutputFlows", ReplyAction="http://www.elma-bpm.ru/api/Workflow/TaskStandardOutputFlowsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/TaskStandardOutputFlows", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData TaskStandardOutputFlows(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/TaskStandardOutputFlows", ReplyAction="http://www.elma-bpm.ru/api/Workflow/TaskStandardOutputFlowsResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> TaskStandardOutputFlowsAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/TasksInfo", ReplyAction="http://www.elma-bpm.ru/api/Workflow/TasksInfoResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/TasksInfo", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData TasksInfo(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/TasksInfo", ReplyAction="http://www.elma-bpm.ru/api/Workflow/TasksInfoResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> TasksInfoAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTask", ReplyAction="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTask", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData ExecuteUserTask(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTask", ReplyAction="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> ExecuteUserTaskAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskAsync", ReplyAction="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskAsyncResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskAsync", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData ExecuteUserTaskAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskAsync", ReplyAction="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskAsyncResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> ExecuteUserTaskAsyncAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskStatus", ReplyAction="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskStatusResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskStatus", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData ExecuteUserTaskStatus(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskStatus", ReplyAction="http://www.elma-bpm.ru/api/Workflow/ExecuteUserTaskStatusResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> ExecuteUserTaskStatusAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartableProcesses", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartableProcessesResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/StartableProcesses", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData StartableProcesses(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartableProcesses", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartableProcessesResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartableProcessesAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartProcessForm", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartProcessFormResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/StartProcessForm", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData StartProcessForm(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartProcessForm", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartProcessFormResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartProcessFormAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartProcess", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartProcessResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/StartProcess", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData StartProcess(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartProcess", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartProcessResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartProcessAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartProcessAsync", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartProcessAsyncResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(TechnologicalRunPG.WorkflowService.PublicServiceException), Action="http://www.elma-bpm.ru/api/Workflow/StartProcessAsync", Name="PublicServiceExceptionFault")]
        TechnologicalRunPG.WorkflowService.WebData StartProcessAsync(TechnologicalRunPG.WorkflowService.WebData data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.elma-bpm.ru/api/Workflow/StartProcessAsync", ReplyAction="http://www.elma-bpm.ru/api/Workflow/StartProcessAsyncResponse")]
        System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartProcessAsyncAsync(TechnologicalRunPG.WorkflowService.WebData data);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WorkflowChannel : TechnologicalRunPG.WorkflowService.Workflow, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WorkflowClient : System.ServiceModel.ClientBase<TechnologicalRunPG.WorkflowService.Workflow>, TechnologicalRunPG.WorkflowService.Workflow {
        
        public WorkflowClient() {
        }
        
        public WorkflowClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WorkflowClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WorkflowClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WorkflowClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TechnologicalRunPG.WorkflowService.WebData TaskStandardOutputFlows(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.TaskStandardOutputFlows(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> TaskStandardOutputFlowsAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.TaskStandardOutputFlowsAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData TasksInfo(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.TasksInfo(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> TasksInfoAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.TasksInfoAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData ExecuteUserTask(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.ExecuteUserTask(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> ExecuteUserTaskAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.ExecuteUserTaskAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData ExecuteUserTaskAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.ExecuteUserTaskAsync(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> ExecuteUserTaskAsyncAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.ExecuteUserTaskAsyncAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData ExecuteUserTaskStatus(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.ExecuteUserTaskStatus(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> ExecuteUserTaskStatusAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.ExecuteUserTaskStatusAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData StartableProcesses(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartableProcesses(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartableProcessesAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartableProcessesAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData StartProcessForm(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartProcessForm(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartProcessFormAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartProcessFormAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData StartProcess(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartProcess(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartProcessAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartProcessAsync(data);
        }
        
        public TechnologicalRunPG.WorkflowService.WebData StartProcessAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartProcessAsync(data);
        }
        
        public System.Threading.Tasks.Task<TechnologicalRunPG.WorkflowService.WebData> StartProcessAsyncAsync(TechnologicalRunPG.WorkflowService.WebData data) {
            return base.Channel.StartProcessAsyncAsync(data);
        }
    }
}
