# CloudTech Android App & Widget

CloudTech (クラウドテック) の主要ページへ、ホーム画面ウィジェットやアプリからワンタップで即座にアクセスできるAndroidアプリケーションです。

## 主な特徴

- 🚀 **Chrome Custom Tabs 連携**: Chromeのログイン状態・セッション・Cookieをそのまま保持して開くため、毎回ログインし直す手間がありません。
- 📱 **ホーム画面ウィジェット (4x1 / 4x2)**: ホーム画面に配置するだけで、4つの主要コンテンツにダイレクトアクセス可能。
- ⚡ **クイックショートカット (App Shortcuts)**: アプリアイコンを長押しして直接各ページを開けます。
- 🎨 **Material 3 デザイン**: ダークモード完全対応のモダンで洗練されたUI。

## 接続先URL一覧

| コンテンツ | URL | 説明 |
| :--- | :--- | :--- |
| **ホーム** | `https://kws-cloud-tech.com/` | 公式トップページ・マイページ |
| **AWSコース一覧** | `https://kws-cloud-tech.com/courses/aws-course/` | 実践的なAWS学習カリキュラム |
| **資格対策動画** | `https://kws-cloud-tech.com/lessons/cert-video-intro/` | 認定試験対策の動画講義 |
| **新資格対策コース** | `https://kws-cloud-tech.com/courses/new-cert/` | 最新のAWS認定試験対策コース |

---

## APKの入手・インストール方法

### 方法 1: GitHub Actions による自動ビルド（推奨）
このリポジトリにプッシュすると、GitHub Actions（`.github/workflows/build-apk.yml`）が自動で起動し、Android APK（`app-debug.apk` / `app-release.apk`）をビルドします。

1. GitHubリポジトリの **「Actions」** タブを開きます。
2. 最新のワークフロー実行結果をクリックします。
3. ページ下部の **Artifacts** から `CloudTech-Release-APK` をダウンロードします。
4. Android端末でダウンロードしたAPKファイルを開き、画面の指示に従ってインストールしてください。
   （※「提供元不明のアプリのインストール」の許可を求められた場合は許可してください）

### 方法 2: Android Studio でのビルド
1. Android Studio でこのリポジトリのディレクトリを開きます。
2. Gradle Sync が完了したら、メニューの `Build` > `Build Bundle(s) / APK(s)` > `Build APK(s)` をクリックします。
3. `app/build/outputs/apk/debug/app-debug.apk` が生成されます。

---

## 使い方

1. アプリをインストールして起動すると、各ページへのカード型リンクリストが表示されます。
2. カードをタップすると、Chrome Custom Tabs で該当ページがスムーズに開きます。
3. ホーム画面の空いている場所を長押しし、**「ウィジェット」** を選択して **「CloudTech」** を配置すると、いつでもホーム画面から1タップで受講・学習を再開できます。
