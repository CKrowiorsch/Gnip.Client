$erroractionpreference = "Stop"
properties {
    $location = (get-location);
    $outdir = (join-path $location "Build");
    $bindir = (join-path $outdir "Bin");
    $artifactsdir = (join-path $outdir "Artifacts");
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

  exec {.nuget/nuget.exe restore}


	exec { msbuild /nologo /v:minimal /t:rebuild /p:"Configuration=Release;OutputPath=$bindir/Gnip.Client/;SolutionDir=$solution/" "Source/Gnip.Client/Gnip.Client.csproj" }
	exec { msbuild /nologo /v:minimal /t:rebuild /p:"Configuration=Release;OutputPath=$bindir/Gnip.Model/;SolutionDir=$solution/" "Source/Gnip.Model/Gnip.Model.csproj" }
	exec { msbuild /nologo /v:minimal /t:rebuild /p:"Configuration=Release;OutputPath=$bindir/NActivityStream/;SolutionDir=$solution/" "Source/NActivityStream/NActivityStream.csproj" }}


task Test -depends Rebuild {
    [void](mkdir $artifactsdir)
    exec { .nuget\nuget.exe restore }
    exec {.nuget\nuget install Machine.Specifications.Runner.Console -OutputDirectory Packages}
    $mspecdir = (resolve-path ".\Packages\Machine.Specifications.Runner.Console.0.*\")
    $mspec = @("$mspecdir\tools\mspec-x86-clr4.exe", "--xml", "$artifactsdir\mspec-results.xml", "--html", "$artifactsdir\mspec-results.html");

    foreach($testProj in (dir -Filter ".\Source\*.Tests")){
      exec { msbuild /nologo /v:m /t:rebuild /p:"Configuration=Release;OutputPath=$bindir/$testProj" "Source/$testProj/$testProj.csproj" }
      $mspec += "$bindir\$testProj\$testProj.dll";
    }

  exec { &([scriptblock]::create($mspec -join ' ')) }

    $xslt = New-Object System.Xml.Xsl.XslCompiledTransform;
    $xslt.Load("Packages\MSpecToJUnit\MSpecToJUnit.xlt");
    $xslt.Transform("$artifactsdir\mspec-results.xml", "$artifactsdir\junit-results.xml");
}

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
