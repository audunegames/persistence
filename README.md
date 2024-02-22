# Audune Persistence

This repository contains a Unity component to store persistent data on disk. This data can be simple states or objects serialized to states.

See the [wiki](https://github.com/audunegames/persistence/wiki) of the repository to get started with the package.

## Installation

Audune Persistence can be best installed as a git package in the Unity Editor using the following steps:

* In the Unity editor, navigate to **Window > Package Manager**.
* Click the **+** icon and click **Add package from git URL...**
* Enter the following URL in the URL field and click **Add**:

```
https://github.com/audunegames/persistence.git
```

Note that this package uses other packages from Audune Games, so make sure to add them to your project:

* `com.audune.utils.unityeditor` ([GitHub repository](https://github.com/audunegames/unityeditor-utils.git))

Note that this package uses packages from the OpenUPM registry, so make sure to add a scoped registry with the URL `https://package.openupm.com` and the following scopes before installing this package:

* `net.tnrd.messagepack`

## License

This package is licensed under the GNU GPL 3.0 license. See `LICENSE.txt` for more information.
