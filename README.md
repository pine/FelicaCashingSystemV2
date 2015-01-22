Felica Cashing System V2
========================

## 概要
非接触 IC カード FeliCa を用いた組織内電子決済システムです。

## 開発言語
- C#
- XAML

## 開発環境
- Windows 7 / 8.1
- Visual Studio 2013 Professional
- SONY PaSoRi RC-S380

## 利用しているライブラリ
- .NET Framework 4.5
  - Windows Forms (一部のみ)
  - WPF
- MashApps.Metro ([カスタマイズ版](https://github.com/pine613/MahApps.Metro/tree/felica_master) を使用)
- [MongoDB](http://www.mongodb.org) (v2.6)
- iTextSharp
- Adobe Acrobat 7.0 Browser Control Type Library 1.0
- PC/SC (WinScard.dll)

## ビルド
ソリューションを Visual Studio で開き、構成を **Release** にしてビルドしてください。

## インストール
ビルド結果をインストール先にコピーしてください。
起動には、Adobe Reader と PaSoRi のドライバが必要です。

## 関係するプロジェクト
- [FelicaSharp](https://github.com/pine613/FelicaSharp)<br />
  カードリーダー (PaSoRi) を C# から利用するためのライブラリ
- [FelicaDataV2](https://github.com/pine613/FelicaDataV2)<br />
  Felica Cashing System V2 のデータベースレイヤを担当するモジュール
- [FelicaCashingSystemV2_Settings](https://github.com/RobotClubKut/FelicaCashingSystemV2_Settings)<br />
  ロボット倶楽部で運用している Felica Cashing System V2 の設定 (非公開)