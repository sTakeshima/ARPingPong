# Ar-Ping-Pong

## 概要

スマートフォンだけで卓球をする。
触覚 x AR = AR Activity

## 構成

- スマートフォン： Android 8.0 Oreo 以降 ※ iOS は今回対象外
- ネットワーク環境
- AR Ping Pong アプリ

## 技術要素

- Unity: Game Engine 及び 開発プラットフォーム
- Android SDK
- Google ARCore: Android 向けの AR ライブラリ(V1.4.0～v1.7.0)
- Cloud Anchors API: Google Cloud Platform の ARCore API
- Programming Languages
  - C#: Unity 側の実装
  - Kotlin v1.32: Android 側の実装
- e.t.c.

### 技術選定

AR 上で卓球を実現するために検討したこと

- Unity: AR, VR, MR, Projection Mapping,  Development platform
- Kinect: Motion capture device
- Leap Motion: Motion capture device
- Pepper's ghost: AR visualization
- Projection Mapping: AR Visualization
- North Star: Open source AR Head mount display
- Nintendo Switch Joy-Con: Hapticable Controller device as racket
- Google ARCore: AR Library for Android
- Apple ARKit: AR Libirary for iOS
- Qualcomm Vuforia: Multiplatform AR Library
- OpenCV: Computer vision library
- TECHTILE Toolkit: For Haptics

#### Concept（というか選定基準）

- 成果物は手軽に利用できること（特殊な設備は極力避けたい）
- 研修終了後も個人で技術的な遊びが続けられること
- 必要なデバイスの調達が安価で手軽なこと
- ハードルの低さ（入門の情報が多い、書籍だったり日本語の技術情報）
- 技術的に新しいこと

#### 開発環境

Unity vs Unreal Engine vs Native

- Kinect, Leap Motion, HMD など多くのデバイスは共通して Unity か Unreal Engine の SDK を出しており、方針が変わって異なるデバイスを使うことになっても潰しが効くのでどちらか。
- どちらかというと Unity の方が SDK 作られる率が高い（個人の感覚）
- 基本的な使用言語の違い(やってみたい言語次第だけど、会社だと Microsoft にどっぷりだし C# ？ Java の人にも取っつきやすいので学習していきやすい)
  - Unity: C#, JavaScript
  - Unreal Engine: C++, C#?
  - Native: 各言語に依存
- 書籍の出版数を見ても Unity の方が多い。
- はい、Unity。

#### AR 卓球実現方式

Motion Capture(Kinect/Leap Motion) vs HMD vs Smartphone(Android/iOS)

- 成果物は手軽に利用できるか？
  - [ ] Motion Capture & HMD: 同じ理由で、PC だったりデバイスの設定が必要なので手軽ではない
  - [x] Smartphone: 新旧あれど、ほとんど誰でも持っている
- 研修後も、続けられる？
  - [ ] Motion Capture & HMD: 買えばね
  - [x] Smartphone: OK

<!-- TODO 飽きた -->

## 仕様

- 同じ AR 空間（卓球の試合）を複数人で共有できる
  - スマートフォンの画面を通して AR 空間を見ることができる
  - プロジェクションマッピングによってスマートフォンがない人も AR 空間を見ることができる※
- スマートフォンをラケットとしてボールを打つことができる
  - ボールとラケットが当たった時の打感があり、仮想のオブジェクトに干渉したことを実感できる
- サーバはスマートフォンで認識した手からボールを出し、サーブを打つことができる

### 卓球

- 卓球台（ネット含む）: スマートフォンのカメラで検出した平面をもとにした仮想オブジェクト
- ピンポン玉： 仮想オブジェクト
- ラケット： プレイヤーのスマートフォン


## Actors

"AR 卓球" を遊ぶとき、操作者は以下のどちらかに分類される。

- Player: AR 卓球を行う人
- Audience: AR 卓球を観戦する人

### Player

Player になる条件は、

- Host となり、AR World を作成する
- Host が作成した、AR World に Player として参加する

### Audience

Audience になる条件は、

- Host が作成した、AR World に Audience として参加する


## Objects

"AR 卓球" において、画面に表示されるオブジェクトを以下のように定義する。

- 仮想オブジェクト: 現実世界に存在しない、画面上だけに表示されるオブジェクト
  - 例：ボール、卓球台（？プロトタイプ時点ではそう）
- 拡張オブジェクト: 現実世界のオブジェクトの情報をもとに表示するオブジェクト
  - 例：相手のラケット