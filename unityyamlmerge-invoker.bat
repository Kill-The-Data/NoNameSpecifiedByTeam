git config mergetool.unityyamlmerge.cmd "C:\\Program\ Files\\Unity\\Hub\\Editor\\2019.3.12f1\\Editor\\Data\\Tools\\UnityYAMLMerge.exe merge -p \"$BASE\" \"$REMOTE\" \"$LOCAL\" \"$MERGE\""
git config mergetool.unityyamlmerge.trustExitCode false
git config merge.tool unityyamlmerge

git mergetool