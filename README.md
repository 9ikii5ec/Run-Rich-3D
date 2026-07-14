# Run Rich 3D

Мобильный раннер, где игрок бежит по сплайну, собирает деньги и проходит через врата социальных статусов. Игра создана на Unity 6 (URP).

## Жанр

Endless runner / runner с выбором пути. Игрок автоматически бежет вперёд, управление — свайп влево/вправо.

## Игровой цикл

1. Игрок стартует со статусом "Poor" (0 денег)
2. Автоматически бежит вперёд по сплайн-пути
3. Свайпом влево/вправо уклоняется от препятствий и собирает деньги
4. Деньги меняют социальный статус: Poor → Worker → Middle → Rich → Millionaire
5. При смене статуса меняется 3D-модель игрока и проигрывается звук
6. Уровень заканчивается на финише, загружается следующий

## Требования

- **Unity 6** (6000.3.18f1)
- **URP** (Universal Render Pipeline 17.3.0)
- **New Input System** (1.19.0)

## Структура проекта

```
Assets/_RunRish3D/
├── Animations/          # Анимации дверей, сбора предметов, UI-курсора
├── Data/                # ScriptableObject'ы (события, настройки статусов, список уровней)
├── Material/            # Материалы (пол, двери, туман, небо и т.д.)
├── Prefabs/             # Префабы (игрок, деньги, бутылки, двери, финиш, чекпоинты)
├── Scenes/
│   └── SampleScene.unity
├── Scripts/
│   ├── Data/            # Enum'ы и ScriptableObject'ы данных
│   ├── Events/          # Шина событий (GameEvents)
│   ├── Interfaces/      # IInteractable
│   ├── Items/           # Предметы (деньги, алкоголь, врата, вращающиеся объекты)
│   ├── Level Manager/   # Управление уровнями (загрузка, прогресс, рандомизация)
│   ├── Player/          # Игрок (движение, инвентарь, визуал, детектор коллизий)
│   ├── Triggers/        # Триггеры (чекпоинты, двери)
│   └── UI/              # Игровой интерфейс
├── Shader/              # Кастомные шейдеры
├── Textures/            # Текстуры
└── Test - Programmer/   # Арт-ассеты (модели, материалы, спрайты, звуки)
```

## Архитектура

### Паттерн: Event Bus на ScriptableObject

Вся коммуникация между системами идёт через `GameEvents` — ScriptableObject с C#-событиями. Скрипты не знают друг о друга, только о `GameEvents`.

```
GameEvents (ScriptableObject)
├── OnGameStarted          — первый тап игрока
├── OnMoneyCollected(int)  — сбор/потеря денег
├── OnStatusChanged(StatusType) — смена социального статуса
├── OnMoneyCountChanged(int)    — обновление счёта
├── OnDistanceChanged(int)      — дистанция (каждый кадр)
└── OnGatePassed(GateType)      — прохождение через врата
```

### Потоки данных

```
[Ввод] PlayerMovement → GameEvents.GameStarted()
                       → GameEvents.DistanceChanged()

[Сбор] MoneyItem / AlcoholItem → GameEvents.MoneyCollected()
      GateItem → GameEvents.GatePassed()

[Инвентарь] PlayerInventory → MoneyCollected → MoneyCountChanged
                                         → StatusChanged (если статус изменился)

[Визуал] PlayerVisuals → StatusChanged → смена модели + звук

[UI] GameUI → GameStarted → скрытие стартового экрана
              DistanceChanged → обновление текста дистанции

[Уровни] LevelManager → загрузка префабов уровней из LevelsList
```

### Система статусов

Определяется `StatusSettings` (ScriptableObject) с порогами денег:

| Статус     | Мин. денег | Модель |
|------------|-----------|--------|
| Poor       | 0         | model[0] |
| Worker     | 20        | model[1] |
| Middle     | 50        | model[2] |
| Rich       | 100       | model[3] |
| Millionaire| 200       | model[4] |

### Система уровней

- Уровни — префабы, хранятся в `LevelsList` (ScriptableObject)
- `LevelManager` — синглтон, загружает/перезагружает уровни через `Instantiate`
- Поддержка последовательного и случайного прохождения
- Прогресс сохраняется в `PlayerPrefs`

### Ввод

Unity New Input System. Привязки:
- `<Pointer>/press` — нажатие/отпускание (работает на мыши и тачскрине)
- `<Pointer>/position` — позиция пальца/курсора

Движение: автоматический бег по сплайну (Unity Splines), свайп влево/вправо — боковое смещение.

## Кастомные шейдеры

| Шейдер | Назначение |
|--------|-----------|
| `WorldSpaceUV` | UV-маппинг из мировых координат (для бесконечного пола) |
| `ProximityFog` | Туман по расстоянию до камеры |

## Сторонние библиотеки

| Библиотека | Использование |
|-----------|---------------|
| **DOTween** (Demigiant) | Анимации (вращение объектов, чекпоинты) |
| **TextMesh Pro** | UI-текст (счёт дистанции) |

## Скрипты

| Скрипт | Роль |
|--------|------|
| `PlayerMovement` | Движение игрока по сплайну, ввод, старт игры |
| `PlayerInventory` | Управление деньгами и статусом |
| `PlayerVisuals` | Смена модели и звука при смене статуса |
| `PlayerTriggerDetector` | Мост между физикой и `IInteractable` |
| `MoneyItem` | Сбор денег (+ значение) |
| `AlcoholItem` | Штраф (- значение) |
| `GateItem` | Врата социального выбора |
| `RotatingItem` | Вращение предметов (DOTween) |
| `DoorTrigger` | Открытие дверей (Animator) |
| `CheckPointTrigger` | Чекпоинт-флаг |
| `LevelManager` | Загрузка и прогресс уровней |
| `LevelManagerEditor` | Кастомный инспектор для LevelManager |
| `LevelsList` | ScriptableObject со списком уровней |
| `Level` | Корневой компонент уровня (spawn point) |
| `GameUI` | Стартовый экран и HUD дистанции |
| `GameEvents` | Глобальная шина событий |
| `StatusSettings` | Пороги социальных статусов |

## Билд

- **Android**: IL2CPP, ARM64, Min SDK 25, Vulkan + OpenGL ES 3.0
- **iOS**: Min iOS 15.0
- **Windows**: x86_64
