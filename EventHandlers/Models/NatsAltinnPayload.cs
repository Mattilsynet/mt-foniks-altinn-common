using System;
using System.Collections.Generic;
namespace MtAltinnCommon.Models;

public class NatsAltinnPayload
{
    public List<DataModel>? Data { get; set; } = [];
    public List<Attachment>? Attachments { get; set; } = [];
}