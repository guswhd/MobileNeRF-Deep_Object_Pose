# MobileNeRF in Unity (Demo)

## 프로젝트 개요

이 프로젝트는 Unity 환경에서 **MobileNeRF(Neural Radiance Fields)** 기술을 활용하여 2D 이미지로부터 3D 장면을 렌더링하는 방법을 시연하는 데모입니다.

`com.doji.mobilenerf` 커스텀 패키지를 사용하여 NeRF 데이터를 Unity 씬으로 가져오고, 상호작용 가능한 3D 에셋으로 구현하는 과정을 포함하고 있습니다.

## 주요 기능

-   MobileNeRF 데이터 임포트 및 렌더링
-   Unity C# 스크립트를 통한 NeRF 씬 제어
-   샘플 데이터(`real2`)를 활용한 실제 구현 예시 포함

## 시작하기

### 요구 사양

-   **Unity Editor 버전:** `2022.3.11f1`

### 실행 방법

1.  이 프로젝트를 Unity Hub를 통해 엽니다.
2.  `Assets/Scenes/SampleScene.unity` 씬을 엽니다.
3.  `Assets/MobileNeRF Data/real2/real2.prefab` 프리팹을 씬에 배치하여 데모를 확인할 수 있습니다.

## 프로젝트 구조

-   `Assets/`: 프로젝트의 주요 에셋이 저장된 폴더입니다.
    -   `MobileNeRF Data/`: `real2`와 같은 샘플 NeRF 데이터셋을 포함합니다.
    -   `Scenes/`: `SampleScene.unity` 등 데모 씬이 포함되어 있습니다.
-   `Packages/`: 프로젝트의 패키지 종속성이 관리됩니다.
    -   `com.doji.mobilenerf/`: MobileNeRF 임포터 및 렌더러의 핵심 로직을 담고 있는 커스텀 패키지입니다.
-   `ProjectSettings/`: Unity 프로젝트의 전반적인 설정 파일들이 위치합니다.
