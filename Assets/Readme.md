# Assets

## はじめに

Assets/ に必要なパッケージをインポートしてください。
現在は、**"ARCore"** (オプションで **"ARKit"**) が必要です。

以下のバージョンをダウンロードしてください。

- ARCore([GitHub](https://github.com/google-ar/arcore-unity-sdk/releases)): **1.7.0**
- (Optional) ARKit: **2.0**(commit: [53b3d3b](https://bitbucket.org/Unity-Technologies/unity-arkit-plugin/commits/53b3d3b059f2dc2de4fe9b1c62e229ca5077aa5b))
  - See [repository](https://bitbucket.org/Unity-Technologies/unity-arkit-plugin)

**追加**

- Unity-Swift([GitHub](https://github.com/miyabi/unity-swift)): iOS版をビルドする時には必須です。
- Poly Toolkit for Unity([GitHub](https://github.com/googlevr/poly-toolkit-unity/releases)): **1.1.2**

## インポート方法

### ARCore

- Download unitypackage. [here](https://github.com/google-ar/arcore-unity-sdk/releases)
- Assets > Import Package > Custom Package...
- Select "arcore-unity-sdk-{version}.unitypackage"

### ARKit

- Clone "Unity-ARKit-Plugin"
- Copy "Unity-ARKit-Plugin/Assets/UnityARKitPlugin" into "ar-ping-pong/Assets/"