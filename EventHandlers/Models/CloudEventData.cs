using System.Collections.Generic;
using System;

namespace MtAltinnCommon.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string? id { get; set; }
        public string? instanceGuid { get; set; }
        public string? dataType { get; set; }
        public string? filename { get; set; }
        public string? contentType { get; set; }
        public string? blobStoragePath { get; set; }
        public SelfLinks? selfLinks { get; set; }
        public int size { get; set; }
        public object? contentHash { get; set; }
        public bool? locked { get; set; }
        public object? refs { get; set; }
        public bool? isRead { get; set; }
        public List<object>? tags { get; set; }
        public object? deleteStatus { get; set; }
        public string? fileScanResult { get; set; }
        public List<Reference>? references { get; set; }
        public DateTime? created { get; set; }
        public string? createdBy { get; set; }
        public DateTime? lastChanged { get; set; }
        public string? lastChangedBy { get; set; }
    }

    public class InstanceOwner
    {
        public string? partyId { get; set; }
        public string? personNumber { get; set; }
        public object? organisationNumber { get; set; }
        public object? username { get; set; }
    }

    public class Process
    {
        public DateTime? started { get; set; }
        public string? startEvent { get; set; }
        public object? currentTask { get; set; }
        public DateTime? ended { get; set; }
        public string? endEvent { get; set; }
    }

    public class Reference
    {
        public string? value { get; set; }
        public string? relation { get; set; }
        public string? valueType { get; set; }
    }

    public class CloudEventData
    {
        public string? id { get; set; }
        public InstanceOwner? instanceOwner { get; set; }
        public string? appId { get; set; }
        public string? org { get; set; }
        public SelfLinks? selfLinks { get; set; }
        public object? dueBefore { get; set; }
        public DateTime? visibleAfter { get; set; }
        public Process? process { get; set; }
        public Status? status { get; set; }
        public object? completeConfirmations { get; set; }
        public List<Datum>? data { get; set; }
        public object? presentationTexts { get; set; }
        public object? dataValues { get; set; }
        public DateTime? created { get; set; }
        public string? createdBy { get; set; }
        public DateTime? lastChanged { get; set; }
        public string? lastChangedBy { get; set; }
    }

    public class SelfLinks
    {
        public string? apps { get; set; }
        public string? platform { get; set; }
    }

    public class Status
    {
        public bool? isArchived { get; set; }
        public DateTime? archived { get; set; }
        public bool? isSoftDeleted { get; set; }
        public object? softDeleted { get; set; }
        public bool? isHardDeleted { get; set; }
        public object? hardDeleted { get; set; }
        public int readStatus { get; set; }
        public object? substatus { get; set; }
    }


}