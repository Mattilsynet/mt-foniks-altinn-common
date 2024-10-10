using System.Collections.Generic;
namespace MtAltinnCommon.Clients.Mats.Models
{
    public class Child
    {
        public string? idstring { get; set; }
        public string? codetype { get; set; }
        public string? code { get; set; }
        public string? parentid { get; set; }
        public int? sequence { get; set; }
        public int? dbsequence { get; set; }
        public string? filter { get; set; }
        public object? codedata { get; set; }
        public object? description { get; set; }
        public List<Codeversion>? codeversions { get; set; }
        public List<object>? children { get; set; }
    }

    public class Codelanguage
    {
        public string? displayname { get; set; }
        public string? validfrom { get; set; }
        public string? lang { get; set; }
    }

    public class Codeversion
    {
        public object? description { get; set; }
        public string? validfrom { get; set; }
        public object? versiondata { get; set; }
        public object? url { get; set; }
        public object? validto { get; set; }
        public List<Codelanguage>? codelanguages { get; set; }
    }

    public class Result
    {
        public string? idstring { get; set; }
        public string? codetype { get; set; }
        public string? code { get; set; }
        public string? parentid { get; set; }
        public int? sequence { get; set; }
        public int? dbsequence { get; set; }
        public object? filter { get; set; }
        public object? codedata { get; set; }
        public object? description { get; set; }
        public List<Codeversion>? codeversions { get; set; }
        public List<Child>? children { get; set; }
    }

    public class Code
    {
        public List<Result>? results { get; set; }
    }

}