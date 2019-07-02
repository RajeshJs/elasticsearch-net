﻿namespace Scripts

[<AutoOpen>]
module Projects = 
    type DotNetFrameworkIdentifier = { MSBuild: string; Nuget: string; DefineConstants: string; }

    type DotNetFramework = 
        | NetStandard2_0
        | Net461
        | NetCoreApp2_1
        static member All = [NetStandard2_0; Net461]
        static member AllTests = [NetCoreApp2_1; Net461] 
        member this.Identifier = 
            match this with
            | NetStandard2_0 -> { MSBuild = "netstandard2.0"; Nuget = "netstandard2.0"; DefineConstants = ""; }
            | NetCoreApp2_1 -> { MSBuild = "netcoreapp2.1"; Nuget = "netcoreapp2.1"; DefineConstants = ""; }
            | Net461 -> { MSBuild = "net461"; Nuget = "net461"; DefineConstants = ""; }

    type Project =
        | Nest
        | ElasticsearchNet
        | NestJsonNetSerializer
        | NestUpgradeAssistant
        | ElasticsearchNetVirtual
        
    type PrivateProject =
        | Tests
        | DocGenerator
        | ApiGenerator
        
    type DependencyProject = 
        | JsonNet 

    type DotNetProject = 
        | Project of Project
        | PrivateProject of PrivateProject
        | DepencyProject of DependencyProject

        static member All = 
            seq [
                Project Project.ElasticsearchNet; 
                Project Project.Nest; 
                Project Project.NestJsonNetSerializer;
                Project Project.NestUpgradeAssistant; 
                Project Project.ElasticsearchNetVirtual; 
                PrivateProject PrivateProject.Tests
            ]

        static member AllPublishable = 
            seq [
                Project Project.ElasticsearchNet; 
                Project Project.Nest; 
                Project Project.NestJsonNetSerializer;
                Project Project.NestUpgradeAssistant;
                Project Project.ElasticsearchNetVirtual;
            ] 
        static member Tests = seq [PrivateProject PrivateProject.Tests]
        
        member this.MergeDependencies=
            match this with 
            | Project Nest -> [Project Project.Nest; ]
            | _ -> []

        member this.VersionedMergeDependencies =
            match this with 
            | Project Nest -> [Project Project.Nest; Project Project.ElasticsearchNet; ]
            | Project NestJsonNetSerializer -> [Project NestJsonNetSerializer; Project Project.Nest; Project Project.ElasticsearchNet ]
            | Project ElasticsearchNet -> [Project ElasticsearchNet]
            | _ -> []

        member this.Name =
            match this with
            | Project Nest -> "Nest"
            | Project ElasticsearchNet -> "Elasticsearch.Net"
            | Project NestJsonNetSerializer -> "Nest.JsonNetSerializer"
            | Project NestUpgradeAssistant -> "Nest.7xUpgradeAssistant"
            | Project ElasticsearchNetVirtual -> "Elasticsearch.Net.Virtual"
            | PrivateProject Tests -> "Tests"
            | PrivateProject DocGenerator -> "DocGenerator"
            | PrivateProject ApiGenerator -> "ApiGenerator"
            | DepencyProject JsonNet -> "Newtonsoft.Json"
 
        member this.NugetId =
            match this with
            | Project Nest -> "NEST"
            | Project NestJsonNetSerializer -> "NEST.JsonNetSerializer"
            | Project NestUpgradeAssistant -> "NEST.7xUpgradeAssistant"
            | _ -> this.Name
        
        member this.NeedsMerge = match this with | Project NestJsonNetSerializer -> false | _ -> true
                
        member this.Versioned name version =
            match version with
            | Some s -> sprintf "%s%s" name s
            | None -> name
            
        member this.InternalName =
            match this with
            | Project _ -> this.Name 
            | PrivateProject _ -> sprintf "Elastic.Internal.%s" this.Name
            | DepencyProject JsonNet -> "Elastic.Internal.JsonNet"
                
        static member TryFindName (name: string) =
            DotNetProject.All
            |> Seq.map(fun p -> p.Name)
            |> Seq.tryFind(fun p -> p.ToLowerInvariant() = name.ToLowerInvariant())

    type DotNetFrameworkProject = { framework: DotNetFramework; project: DotNetProject }
    let AllPublishableProjectsWithSupportedFrameworks = seq {
        for framework in DotNetFramework.All do
        for project in DotNetProject.AllPublishable do
            yield { framework = framework; project= project}
        }

