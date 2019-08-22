# Controll Physics, Sound and Haptics.

## Design

### Classes

- CollisionController : MonoBehavior
- CollisionActionBehaviour : MonoBehavior
- SimpleSoundCollisionActionBehaviour : CollisionActionBehaviour
- SimpleHapticCollisionActionBehaviour : CollisionActionBehaviour
- CollisionActionResult

### Detail Classes

- CollisionController は、RigidBody や Collider がアタッチされた GameObject にアタッチする
- CollisionController は、いくつかの CollisionActionBehaviour を持つ(持たなくてもよくて、その場合は何もしない)
- CollisionController は、OnCollisionEnter 時に、保持する CollisionActionBehaviour を追加された順に処理する
- CollisionActionBehaviour は、OnCollisionAction を仮想メソッドとするクラスである
- OnCollisionAction は、引数に CollisionActionResult オブジェクトを受け取り、処理を行い、CollisionResult を更新して返す
- CollisionActionBehaviour は、CollisionActionResult が Null でも問題無いように実装する※
- 一番最初の CollisionActionResult には、自身の GameObject と衝突相手の Collider を入れておく
- 各 CollisionActionBehaviour は、衝突相手を処理するかどうか自分で判断する

## Usage

- CollisionActionBehaviour#OnCollisionAction では、何もしない場合は、基本的に受け取った CollisionActionResult をそのまま Return する
- CollisionActionBehaviour の登録順は、必要ならば、物理演算、音、触覚が良いかなと思う
  - 物理演算で、強さだったり、どんな当たり方だったか分かり、
  - 当たり方に応じた音データを当たった強さで再生し、
  - その音に応じた触覚データを再生する
  - といった感じにできるかなと。。。


### 触覚を使う場合(Racket とか)

- CollisionController には、物理演算、音、触覚の順に CollisionActionBehaviour を追加する

### 触覚を使わない場合(Table/Stage とか)

- CollisionController には、物理演算、音の順に CollisionActionBehaviour を追加する