# CloudTech Multi-Platform Access Suite ☁️

CloudTech (クラウドテック) の主要コンテンツへ、スマートフォンでもPCでもワンタップ／ワンクリックで即座にアクセスできるマルチプラットフォーム・ポータルツールキットです。

---

## 接続先URL一覧

| コンテンツ | URL | 説明 |
| :--- | :--- | :--- |
| **🏠 ホーム** | `https://kws-cloud-tech.com/` | 公式トップページ・マイページ |
| **☁️ AWSコース一覧** | `https://kws-cloud-tech.com/courses/aws-course/` | 実践的なAWS学習カリキュラム |
| **🎬 資格対策動画** | `https://kws-cloud-tech.com/lessons/cert-video-intro/` | 認定試験対策の動画講義 |
| **🎓 新資格対策コース** | `https://kws-cloud-tech.com/courses/new-cert/` | 最新のAWS認定試験対策コース |

---

## 提供ツール（3つの利用スタイル）

### 1. 📱 Android アプリ & ホーム画面ウィジェット
- **特徴**: 4x1/4x2 ホーム画面ウィジェット、アイコン長押しショートカット、Chrome Custom Tabs連携（Chromeのログイン状態を保持）
- **入手方法**: [GitHub Releases v1.0.0](https://github.com/cannedcountry-star/cloudtechapp/releases/tag/v1.0.0) から `app-release.apk` をダウンロードして端末にインストールしてください。

### 2. 🖥️ Windows デスクトップ ウィジェット (`windows-widget/`)
- **特徴**: デスクトップ上の好きな場所に置けるフローティングウィジェット。最前面ピン留め（📌）、タスクトレイ常駐対応。クリックでChrome/規定ブラウザを自動起動。
- **入手方法**: `windows-widget/CloudTechWidget.exe` をダブルクリックするだけですぐに使えます（インストール不要・約12KB）。

### 3. 🌐 Google Chrome 拡張機能 (`chrome-extension/`)
- **特徴**: Chromeのツールバー右上に常駐し、クリックするとポップアップメニューからワンクリックで各ページを新しいタブで開きます。
- **導入方法**: Chromeの `chrome://extensions/` でデベロッパーモードをONにし、「パッケージ化されていない拡張機能を読み込む」から `chrome-extension` フォルダを選択してください。

---

## プロジェクト構成

```
cloudtechapp/
├── app/                  # Android アプリソースコード (Kotlin + Material 3)
├── windows-widget/       # Windows デスクトップ ウィジェット (.exe & C# ソース)
├── chrome-extension/     # Google Chrome 拡張機能 (Manifest V3)
├── .github/workflows/    # GitHub Actions 自動APKビルドCI/CD
└── README.md
```
