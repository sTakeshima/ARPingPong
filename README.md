# ar-ping-pong

AR PingPong 共同開発用のプロジェクト

## はじめに

初回クローン後に必要な操作を記載します。

1. 適切なブランチをチェックアウトする
2. 必要なパッケージをインポートする [参考](./Assets/Readme.md)
3. "File > Build Settings" で、 Platform を Android に変更する
4. "Player Settings..." を開き、正しく設定されているか確認する
    + [参考](https://developers.google.com/ar/develop/unity/quickstart-android): ARCore 公式の "Configure build setting"
    + アプリの package name は、**com.ricoh.teamhaptic.arpingpong** になっていること
5. CloudAnchors サンプルを利用するときと同様に準備する [参考: About Cloud Anchors ID sharing, Add an API Key](https://developers.google.com/ar/develop/unity/cloud-anchors/quickstart-unity-android)
6. 端末を接続してビルドしてみる

## ディレクトリ構成

- **Assets**
  - **Plugins**: ARCore や ARKit 等のプラグインのインポート先(プラグイン自体は Git 管理対象外)
    - **Readme.md**: インポート方法とインポートするバージョンを記載する※Git 管理対象 
  - **ARPingPong**: アプリ開発ディレクトリ
    - **Scenes**: Unity Scene 置き場
    - **Scripts**: Unity Script 置き場
    - **Models**: .obj とか
    - **Prefabs**: prefab 置き場
    - **Materials**: material 置き場
    - **Configurations**: ARCore, ARKit の設定置き場
    - **等々**
- **Packages**
  - **manifest.json**: その他 Assets とか依存するライブラリとかを記載、基本はいじることはない。
- **Documents**: 開発系のドキュメントとか

## ブランチ運用ルール

- **master**: 清書用
- **develop{version}**: 開発用ブランチ★メインはこれ
- **developPractice**: Git 練習用のブランチ

### Version ルール

- {YYYY}.{MM}.{n}: 例 develop2019.01.1
- 同じバージョンの間は、ARCore / ARKit 等のプラグインバージョンを上げない（破壊的な変更の可能性があるため）
- ARCore や ARKit のバージョンを上げるときは新たにブランチを切る

### Git の練習(developPractice の使い方)

- **developPractice** ブランチには checkout しただけではビルドが通らない状態でコミットしておきます（develop{version} ブランチも同じ運用）
- 各自 **developPractice** から新たにブランチを切って開発をスタート（例：**developPractice_yykatoh** みたいなブランチを作る）
- 以下の手順でビルドが通るようにする
  - "/Assets/Plugins/Readme.md" に記載の方法で、ARCore をインポートする
  - "/Assets/ARPingPong/Scripts/" のスクリプトのコメントアウトを外す
  - ビルドする
- ビルドができたら、変更点をコミットする
  - "/Assets/Plugins" 以下は変更しても Git の管理対象外のはずなのでコミット対象になっていないことを確認する
  - "/Assets/ARPingPong/Scripts" の変更したスクリプトのみ

### Git Hands-On

- [Hands-On](https://gitlab.com/team-haptic/ar-ping-pong/wikis/githandson)

## Dependencies(Plugins)

- [See Assets Readme](./Assets/Readme.md)

## VSCode 関連

### .vscode/settings.json

#### 特定のファイルを表示しない設定

workspace settings に以下を追加すると VSCode 上はすっきりする。
※見えていないだけで変更されているファイルはあるので、コミット時に注意する。

```json
{
    "files.exclude":
    {
        "**/.DS_Store":true,
        "**/.git":true,
        "**/.gitignore":false,
        "**/.gitmodules":true,
        "**/*.booproj":true,
        "**/*.pidb":true,
        "**/*.suo":true,
        "**/*.user":true,
        "**/*.userprefs":true,
        "**/*.unityproj":true,
        "**/*.dll":true,
        "**/*.exe":true,
        "**/*.pdf":true,
        "**/*.mid":true,
        "**/*.midi":true,
        "**/*.wav":true,
        "**/*.gif":true,
        "**/*.ico":true,
        "**/*.jpg":true,
        "**/*.jpeg":true,
        "**/*.png":true,
        "**/*.psd":true,
        "**/*.tga":true,
        "**/*.tif":true,
        "**/*.tiff":true,
        "**/*.3ds":true,
        "**/*.3DS":true,
        "**/*.fbx":true,
        "**/*.FBX":true,
        "**/*.lxo":true,
        "**/*.LXO":true,
        "**/*.ma":true,
        "**/*.MA":true,
        "**/*.obj":true,
        "**/*.OBJ":true,
        "**/*.asset":true,
        "**/*.cubemap":true,
        "**/*.flare":true,
        "**/*.mat":true,
        "**/*.meta":true,
        "**/*.prefab":true,
        "**/*.unity":true,
        "build/":true,
        "Build/":true,
        "Library/":true,
        "library/":true,
        "obj/":true,
        "Obj/":true,
        "ProjectSettings/":true,
        "temp/":true,
        "Temp/":true
    }
}
```