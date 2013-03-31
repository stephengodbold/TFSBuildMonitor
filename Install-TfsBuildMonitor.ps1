param(
    [parameter()]
    [string]
    $repositoryRoot,
    
    [parameter()]
    [string]
    $installRoot = $env:ProgramFiles
)

function Get-MsBuildPath {
    $registryLocation = 'HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0\'    
    $registryEntry = Get-ItemProperty $registryLocation -Name 'MSBuildToolsPath'
    
    return $registryEntry.MSBuildToolsPath
}

function Test-Service {
    $service = Get-Service TfsBuildMonitor -ErrorAction 'SilentlyContinue'
    return $service -ne $null
}

function Install-Service {
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $installPath 
    )

    $serviceExists = Test-Service

    if ($serviceExists) {
        Stop-Service $applicationName
    } 

    $outputPath = Join-Path $repositoryRoot 'BuildMonitor.Service\bin\Release'

    if (-not (Test-Path $installPath)) {
        New-Item $installPath -Type Directory
    }

    Get-ChildItem $outputPath | foreach { Copy-Item $_.FullName $installPath }

    if (-not $serviceExists) {
        $executablePath = Join-Path $installPath 'BuildMonitor.Service.exe'
        New-Service `
            -Name $applicationName `
            -BinaryPathName $executablePath `
            -StartupType Automatic `
            -Credential (Get-Credential) `
            -DisplayName $applicationName `
            -Description 'A build monitor service to indicate success/failure of a build via a Delcomm light'        
    }   
}

function Expand-Zip($SourcePath, $TargetPath) {
    #Unzip code modified from http://bit.ly/fwGjfY               
    $UnzipShell = new-object -com shell.application
	$ZipPackage = $UnzipShell.NameSpace($SourcePath)
	$DestinationFolder = $UnzipShell.NameSpace($TargetPath)
    $DestinationFolder.CopyHere($ZipPackage.Items())
}

function Get-OSVersion {
    $processor = Get-WmiObject Win32_Processor
    if ($processor.AddressWidth -eq 64) {
        return '64'
    }

    return ''
}

function Unzip-DelcomDependency {
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $repositoryRoot,

        [Parameter(Mandatory=$true)]
        [string]
        $installPath
    )

    $OSVersion = Get-OSVersion
    $archiveName = "DelcomDLL$OSVersion.zip"

    $archivePath = Join-Path $repositoryRoot $archiveName

    Expand-Zip $archivePath $installPath
}

if ($repositoryRoot -eq '') {
    $repositoryRoot = Get-Location
}

$applicationName = 'TfsBuildMonitor'
$installPath = Join-Path $installRoot $applicationName

Unzip-DelcomDependency $repositoryRoot $installPath
Install-Service $installPath
Start-Service $applicationName