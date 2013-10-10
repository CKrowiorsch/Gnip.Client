$erroractionpreference = "Stop"
properties {
    $location = (get-location);
    $outdir = (join-path $location "Build");
    $bindir = (join-path $outdir "Bin");
}

task default -depends Help
task deply -depends Help


task Help {
    write-host "To Create Nuget-Packages Locally: psake.cmd 'Create:Nuget'" -ForegroundColor Magenta
}

task Check {
	# nothing
}

task Clean {
	rmdir -force -recurse $outdir -ea SilentlyContinue
}

task Rebuild -depends Clean {
    $solution = get-location;
	exec { msbuild /nologo /v:minimal /t:rebuild /p:"Configuration=Release;OutDir=$bindir/Gnip.Client/;SolutionDir=$solution/" "Source/Gnip.Client/Gnip.Client.csproj" }
	exec { msbuild /nologo /v:minimal /t:rebuild /p:"Configuration=Release;OutDir=$bindir/Gnip.Model/;SolutionDir=$solution/" "Source/Gnip.Model/Gnip.Model.csproj" }
	exec { msbuild /nologo /v:minimal /t:rebuild /p:"Configuration=Release;OutDir=$bindir/NActivityStream/;SolutionDir=$solution/" "Source/NActivityStream/NActivityStream.csproj" }}


task Create:Nuget -depends Rebuild,Check {
    push-location "$bindir/"
       
    copy "$location/Gnip.Client.nuspec" $bindir
    
    $version = ([System.Diagnostics.FileVersionInfo]::GetVersionInfo("$bindir\Gnip.Client\Gnip.Client.dll").productVersion);
    ..\..\.NuGet\NuGet.exe pack "Gnip.Client.nuspec" /version "$version"
	
	copy "$location/Gnip.Model.nuspec" $bindir
    
    $version = ([System.Diagnostics.FileVersionInfo]::GetVersionInfo("$bindir\Gnip.Model\Gnip.Model.dll").productVersion);
    ..\..\.NuGet\NuGet.exe pack "Gnip.Model.nuspec" /version "$version"
	
	copy "$location/NActivityStream.nuspec" $bindir
    
    $version = ([System.Diagnostics.FileVersionInfo]::GetVersionInfo("$bindir\NActivityStream\NActivityStream.dll").productVersion);
    ..\..\.NuGet\NuGet.exe pack "NActivityStream.nuspec" /version "$version"
    
    pop-location
}

