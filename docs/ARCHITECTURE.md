# R18DarkSNS アーキテクチャ設計

## 概要

本システムは、画像・動画投稿中心のR18 SNSアプリケーションです。主に以下の技術で構築します。
- バックエンド: ASP.NET Core WebAPI
- フロントエンド: Blazor WebAssembly
- ストレージ: Azure Blob Storage
- DB: Azure SQL Database

---

## Azure構成

- **App Service**: WebAPI/Blazorホスティング
- **Blob Storage**: 投稿メディア保存
- **SQL Database**: ユーザー/投稿/コメント管理

---

## 初期データモデル例（略）

---

## 運用構造（SuperAdmin/Manager 二階層＋履歴管理の基本設計）（略）

---

## **拡張可能な点**（略）

---

## 今後の拡張プラン（略）

---

## セキュリティ・運用（略）
