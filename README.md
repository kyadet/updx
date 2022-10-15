# updx - light weight updater ... multiplatform pc package distribution manager with transparency
![alt text](https://github.com/kyadet/updx/blob/main/logo.png?raw=true)

Feature
- simple, single binary
- pc multi platform updator
Windows/Mac/Linux
- versioning
- crash reports
- describing setting and generate, easy!


## How to use

1. setup settings.json
 set execute assembly name
 set check require version endpoint/url
 set check maintenance endpoint
 set check announce endpoint
 set check updx selfupdate notice
 set check file check
 set check crc check
 set assembly download url
 set resource download url
 set update / download dialog ui type NONE|ProgressOnly|ProgressAndMessage|SprashWindow
 set icon resource

2. execute generate.bat(generate.exe)

create bin/updx.exe
create bin/updx_uninstall.exe

# Notice
We cannot use .net core , yet.
.net core always thorw exception.
CodeDomProvider.CompileAssemblyFromSource
https://learn.microsoft.com/ja-jp/dotnet/core/compatibility/unsupported-apis

# License
The MIT License

@see [DEPENDENT LICENSES](https://github.com/kyadet/updx/blob/main/LICENSE)

©︎ FurtherSystem Co.,Ltd.