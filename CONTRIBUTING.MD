# HOW DOES ONE CODE

```csharp

//minmial includes, get rid of everything you do not use
using UnityEngine;

//no namespace, uneccessary for Unity tbh

//CamelCase for classes
public class PlayerController : MonoBehaviour {


    //use m_ for private variables
    private Vector3 m_speed;

    //use UPPER_CASE for consts
    public const float MIN_EPSILON = 0.00001f;

    [Header(" --- use headers like this ---")]
    [Tooltip("Think of good tooltips")]
    //CamelCase for publics
    public float Drag = 0.95f;

    // stuff omitted ...

    //functions should be CamelCase
    void Update() {

        //lowerCamelCase for local Vars
        Vector2 mPos = Input.mousePosition;
        // rest omitted ...
    }

}

```

# HOW DOES ONE PUSH

Do not! push to develop or master directly unless you modify only documentation, because of typos or other semantic mistakes! (a file like this)

to push to develop use a feature/ branch

to push to master use a release/ branch once done with the release push it into master and push master into develop to align the repository HEADS, if there is a Merge-Conflict contact a Repository Administrator

# HOW DOES ONE UNITY-SCENE
If you work extensively on the layout of a scene, make sure you are the only one working on that change or copy the entire scene and prefab your changes, when it comes to merge the scene differences contact the Administrators to encorporate the entire change into the repository, if you must work on the Scene, contact the adminstrator beforehand or resolve the merge yourself with unityyamlmerge and test it before pushing:

```bash

# Note for me the path to Unityyamlmerge was:
# "C:\Program Files\Unity\Hub\Editor\2019.3.12f1\Editor\Data\Tools\UnityYAMLMerge.exe"
# so the path becomes
# "C:\\Program\ Files\\Unity\\Hub\\Editor\\2019.3.12f1\\Editor\\Data\\Tools\\UnityYAMLMerge.exe"
# thanks a lot windows
git config mergetool.unityyamlmerge.cmd "<Folder\\Path\ to\ UnityYamlMerge> merge -p \"$BASE\" \"$REMOTE\" \"$LOCAL\" \"$MERGE\""
git config mergetool.unityyamlmerge.trustExitCode false
git config merge.tool unityyamlmerge

git checkout <your-branch>
git merge develop
# you will probably get merge conflicts here!!
git mergetool # use this to resolve them, when it merges scenes it will tell
              # you that the files seems to be unchanged, answer it changed them
              # and open the scene to confirm that

git commit -am"resolved merge conflict"
git push origin <your-branch>


```