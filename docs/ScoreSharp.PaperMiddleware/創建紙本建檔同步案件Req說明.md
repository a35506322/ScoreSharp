# 正附卡片邏輯

### `CardOwner`（正附卡別）

**定義值：**

| 值  | 說明        |
| --- | ----------- |
| 1   | 正卡        |
| 2   | 附卡        |
| 3   | 正卡 + 附卡 |

**💡 說明：**

- 當 `CardOwner = 1` 時：  
  → 表示會有 1 張以上的正卡及正卡人資料，**沒有附卡**。
- 當 `CardOwner = 2` 時：  
  → 表示會有 1 張以上的附卡，以及**正＋附卡人資料**，**沒有正卡**。
- 當 `CardOwner = 3` 時：  
  → 表示會有 1 張以上的正卡及 1 張以上的附卡，以及**正＋附卡人資料**。

---

### `M_CardCount`（正卡卡片數量）

**💡 說明：**

- 當 `CardOwner = 2` 時，`M_CardCount = 0`。
- **不論 `M_CardCount` 為多少，正卡人資料都會產生。**

---

### `S_CardCount`（附卡卡片數量）

**💡 說明：**

- 當 `CardOwner = 1` 時，`S_CardCount = 0`。
- 當 `CardOwner = 3` 時，`S_CardCount` **必須小於等於** `M_CardCount`。

---

### `IsStudent`（正卡是否為學生身份）

**💡 說明：**

- 當 `IsStudent = Y` 時，正卡人資料會產生學生相關資料。
- 當 `IsStudent = N` 時，不會產生學生資料。

**學生相關資料欄位如下：**

- `M_StudSchool`：正卡\_學生就讀學校
- `M_StudScheduledGraduationDate`：正卡\_學生預定畢業日期
- `M_ParentName`：正卡\_家長姓名
- `M_StudentApplicantRelationship`：正卡\_學生與申請人關係
- `M_ParentCompPhone`：正卡\_家長公司電話
- `M_ParentHomePhone`：正卡\_家長居住電話
- `M_ParentMobile`：正卡\_家長行動電話
- `M_ParentLive_ZipCode`：正卡家長居住地址\_郵遞區號
- `M_ParentLive_City`：正卡家長居住地址\_縣市
- `M_ParentLive_District`：正卡家長居住地址\_區域
- `M_ParentLive_Road`：正卡家長居住地址\_路
- `M_ParentLive_Lane`：正卡家長居住地址\_巷
- `M_ParentLive_Alley`：正卡家長居住地址\_弄
- `M_ParentLive_Number`：正卡家長居住地址\_號
- `M_ParentLive_SubNumber`：正卡家長居住地址\_之號
- `M_ParentLive_Floor`：正卡家長居住地址\_樓
- `M_ParentLive_Other`：正卡家長居住地址\_其他

---

### `CardStatus`（卡片狀態）

**定義值：**

| 值    | 說明       |
| ----- | ---------- |
| 20002 | 一次件檔中 |
| 20004 | 二次件檔中 |
| 20007 | 建檔審核中 |

**歷程產生順序：**

- 初始 → 一次件檔中 → 二次件檔中 → 建檔審核中

**💡 說明：**

- 當 `CardStatus = 20002` 時：  
  → 卡片狀態為「一次件檔中」，產出歷程：「初始」+「一次件檔中」

- 當 `CardStatus = 20004` 時：  
  → 卡片狀態為「二次件檔中」，產出歷程：「初始」+「一次件檔中」+「二次件檔中」

- 當 `CardStatus = 20007` 時：  
  → 卡片狀態為「建檔審核中」，產出歷程：「初始」+「一次件檔中」+「二次件檔中」+「建檔審核中」

---

### `SyncStatus`（同步狀態）

**定義值：**

| 值  | 說明 |
| --- | ---- |
| 1   | 修改 |
| 2   | 完成 |

**💡 說明：**

- 當 `SyncStatus = 2` 時：  
  → 完成 API 呼叫後，紙本同步完成，案件狀態為「紙本件\_待審核」

---

### 範例組合：

| CardOwner | M_CardCount | S_CardCount | IsStudent | SyncStatus | 說明                                  |
| :-------: | :---------: | :---------: | :-------: | :--------: | :------------------------------------ |
|     1     |      2      |      0      |     N     |     1      | 兩張正卡，非學生身份，同步狀態為修改  |
|     2     |      0      |      3      |     N     |     1      | 三張附卡，非學生身份，同步狀態為修改  |
|     3     |      3      |      3      |     Y     |     2      | 正卡學生身份，3 正 3 附，紙本同步完成 |
