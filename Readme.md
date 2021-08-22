# .Net Core on Fargate 定期実行処理 サンプルアプリ

## ローカル起動

```
dotnet run
```

## Copilotを使った環境構築・デプロイ手順

### アプリケーションの作成

```
copilot app init [Application Name]
```

### ジョブの作成

```
copilot job init \ 
    --name [Job Name] \
    --app [Application Name] \
    --dockerfile ./Dockerfile \
    --schedule "@every 2m"
```

### デプロイ

```
copilot job deploy --name [Job Name] --env [Env Name]
```
