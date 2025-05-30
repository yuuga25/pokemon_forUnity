**個人開発作品**
# 「Pokémon Battle Simulator」

- [概要](https://github.com/yuuga25/pokemon_forUnity#%E6%A6%82%E8%A6%81)
    - [解説動画](https://github.com/yuuga25/pokemon_forUnity#%E8%A7%A3%E8%AA%AC%E5%8B%95%E7%94%BB-)
- [ゲーム内容](https://github.com/yuuga25/pokemon_forUnity/blob/master/README.md#%E3%82%B2%E3%83%BC%E3%83%A0%E5%86%85%E5%AE%B9)
- [製作意図](https://github.com/yuuga25/pokemon_forUnity#%E5%88%B6%E4%BD%9C%E6%84%8F%E5%9B%B3)
- [使用技術](https://github.com/yuuga25/pokemon_forUnity#%E4%BD%BF%E7%94%A8%E6%8A%80%E8%A1%93)
- [気を付けた・こだわった部分](https://github.com/yuuga25/pokemon_forUnity#%E3%83%87%E3%83%BC%E3%82%BF%E3%83%99%E3%83%BC%E3%82%B9)
    - [ユーザーが面白いと感じる要素を参考にしたゲームから見つける](https://github.com/yuuga25/pokemon_forUnity/edit/master/README.md#%E3%83%A6%E3%83%BC%E3%82%B6%E3%83%BC%E3%81%8C%E9%9D%A2%E7%99%BD%E3%81%84%E3%81%A8%E6%84%9F%E3%81%98%E3%82%8B%E8%A6%81%E7%B4%A0%E3%82%92%E5%8F%82%E8%80%83%E3%81%AB%E3%81%97%E3%81%9F%E3%82%B2%E3%83%BC%E3%83%A0%E3%81%8B%E3%82%89%E8%A6%8B%E3%81%A4%E3%81%91%E3%82%8B)
    - [データベース](https://github.com/yuuga25/pokemon_forUnity#%E3%83%87%E3%83%BC%E3%82%BF%E3%83%99%E3%83%BC%E3%82%B9)
    - [UI](https://github.com/yuuga25/pokemon_forUnity#ui)
- [感想](https://github.com/yuuga25/pokemon_forUnity#%E8%A3%BD%E4%BD%9C%E3%81%97%E3%81%A6%E3%81%BF%E3%81%A6%E6%84%9F%E3%81%98%E3%81%9F%E3%81%93%E3%81%A8%E6%84%9F%E6%83%B3)

## 概要

コンソール向けに発売されている、「**ポケモン**」をソーシャルゲームの形で再現したゲームです。  
開発期間：2021年 8月～9月

※非営利目的です※

### [解説動画 ←](https://youtu.be/9KUepTCOu5s)

## ゲーム内容

コンソール向けに発売されているポケモンのバトル要素と同じです。

## 制作意図

一年次の頃から、ソーシャルゲームのような開発してみたく、その思いを実現させる**技術**と**機会**を得たので、実際に制作してみました。

## 使用技術
**Unity**
  
**PlayFab**

## 気を付けた部分・こだわった部分

### ユーザーが面白いと感じる要素を参考にしたゲームから見つける
>今回は、僭越ながらポケモンのゲームを再現したため、この既存のゲームはどの部分がユーザーにとって面白い要素なのかを考え、
>限りある制作期間の中でどう実装していくかを模索しました。
>
>そして今回このゲームを開発するうえで、ペルソナを
>「**ポケモンを遊んだことはあるがオンライン対戦では遊んだことがない**」
>「**オンライン対戦で遊んではいるが強さに伸び悩んでいる**」この二つに設定しました。
>
>そして、これらのユーザーがポケモンを遊ぶうえで面白いと感じていた部分を想定していたペルソナに近い
>人に実際にヒアリングを行い、今回は実際に「ポケモンの収集」と「ステータスの育成」に力を入れて制作しました。

### データベース
>**PlayFab**を使用するのが初めてだったので、データベースの作成に重点を置き制作しました。  
>どのようにしたら、後々要素を追加した際に引き出しやすい形で、個人製作ではありますが、他人が見やすい・扱いやすいかを考え設計しました。
>
>まだまだ、不慣れな部分もあるため今後もPlayFabは継続的に活用していきたいと思いました。

### UI
>多くのソーシャルゲームでは、画面のほとんどをUIが占めており、製作を始める段階で絶対に怠ってはいけない部分だと感じておりました。  
>**カッコいい**・**可愛い**UIを目指すのは大切ですが、今回はそちらではなくエンジニアとして、ユーザーにとって**使いやすいUI**を目指しました。  
>何回もデバッグをする中で、  
>「使いづらいと感じたものは、直ぐに新しい形に変更しテストをする」  
>ということを繰り返し、操作性の高いUIを目指しました。
>
>また、プレイしてもらわないとわからない部分などもある為、友人や家族にスマートフォン上で遊んでもらい感想をもらうなど
>実際に遊んでもらうことを想定し、開発を行いました。

他にも様々な部分にこだわっておりますので、是非 [解説動画](https://youtu.be/9KUepTCOu5s) でご覧ください。

## 製作してみて感じたこと（感想）
PlayFabを使用して制作するのが初めてであり、PlayFabの仕様に慣れておらずデータベースに改善の余地はあったと感じました。  
基にした作品が「**ポケモン**」だったこともあり、データ数が多く、勉強になった反面とても苦労しました。  
以上の点も含め自分の中で納得がいってないため、近いうちに再度製作したいと考えています。
  
### [次作 ←](https://github.com/yuuga25/DiceQuest_new)  
  
***
2022/01/31
