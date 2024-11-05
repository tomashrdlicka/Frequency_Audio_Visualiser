# Unity Audio Visualizer Project (Frequency mapping)

This project is an audio visualizer built with Unity, designed to transform audio input into a captivating visual display. It features dynamic particle systems, interactive lights, and reactive materials that respond to audio frequency data to create an immersive visual experience. This document outlines the project components, their functions, and setup instructions.

## Table of Contents
1. [Overview](#overview)
2. [Project Structure](#project-structure)
3. [Scripts](#scripts)
   - [AudioSpectrum.cs](#audiospectrumcs)
   - [ParticleAudioVisualizer.cs](#particleaudiovisualizercs)
   - [LightIntensityController.cs](#lightintensitycontrollercs)
   - [ShaderAudioLink.cs](#shaderaudiolinkcs)
   - [ReactiveSphereController.cs](#reactivespherecontrollercs)
4. [Shaders](#shaders)
   - [BassReactiveParticleShader.shader](#bassreactiveparticleshadershader)
   - [MidReactiveParticleShader.shader](#midreactiveparticleshadershader)
   - [HighReactiveParticleShader.shader](#highreactiveparticleshadershader)
   - [ReactiveSphereShader.shader](#reactivesphereshadershader)
5. [Materials](#materials)
   - [BassParticleMaterial](#bassparticlematerial)
   - [MidParticleMaterial](#midparticlematerial)
   - [HighParticleMaterial](#highparticlematerial)
   - [ReactiveSphereMaterial](#reactivespherematerial)
6. [Installation and Setup](#installation-and-setup)
7. [How To Use](#how-to-use)
8. [Song Credit](#song-credit)
9. [License](#license)

## Overview

The Unity Audio Visualizer Project transforms audio data into interactive, real-time visualizations. This project supports a variety of audio-reactive elements, including particle effects, dynamic lighting, and shaders, that respond to different frequency bands (bass, mid, high). This allows for a visually compelling experience synced to music of choice.

## Project Structure

### Key Components
- **Scripts**: Handle audio analysis and control visual components.
- **Shaders**: Provide advanced visual effects for real-time audio response.
- **Materials**: Link shaders with GameObjects to create specific visual effects.
- **Audio Source**: Provides the audio input for the entire visualization.

## Scripts

### AudioSpectrum.cs
**Purpose**: Captures and processes audio data from an `AudioSource` to create a spectrum array used by other scripts.

**Key Features**:
- Supports a configurable `spectrumSize` (64 to 8192) to match project needs.
- Populates a static `spectrum` array for easy access by other scripts.
- Uses Unity’s `GetSpectrumData()` to gather frequency data from the audio source.

**Code Behavior**:
- Initializes the `AudioSource` and the `spectrum` array.
- Updates the `spectrum` array each frame, ensuring real-time audio data is available for other components.

### ParticleAudioVisualizer.cs
**Purpose**: Modifies the behavior of a particle system based on the audio spectrum data, adjusting properties like emission rate, size, and color for bass, mid, and high frequencies.

**Key Features**:
- Controls particle emission, color, size, and trail properties.
- Uses `Mathf.Lerp()` for smooth transitions in response to amplitude changes.
- Configurable parameters for emission and amplitude scaling.

**Detailed Explanation**:
- The script references `AudioSpectrum.spectrum` to gather amplitude data.
- Smooths amplitude changes to prevent abrupt visual transitions.
- Adjusts particle and trail color based on the assigned frequency range, creating a differentiated look for each band.

### LightIntensityController.cs
**Purpose**: Dynamically adjusts the intensity of point lights based on high-frequency audio amplitude for additional visual emphasis.

**Key Features**:
- Smoothly interpolates light intensity using `Mathf.Lerp()` for fluid changes.
- Accepts customizable `baseIntensity` and `intensityMultiplier` values.
- Responds to high-frequency amplitudes for impactful visual highlights.

**How It Works**:
- Fetches high-frequency data from `AudioSpectrum.spectrum`.
- Modulates light intensity to sync with high audio peaks, creating a pulsating effect.

### ShaderAudioLink.cs
**Purpose**: Acts as a bridge between the audio data and material properties, allowing shaders to react in real-time to the audio.

**Key Features**:
- Passes processed audio amplitudes (bass, mid, high) as shader properties.
- Scales and clamps amplitude values to ensure consistent and manageable visual effects.

**How It Integrates**:
- Updates shader properties per frame to create real-time audio-linked visual feedback in materials.

### ReactiveSphereController.cs
**Purpose**: Controls the visual behavior of a reactive sphere that rotates, shifts colors, and applies a vortex effect based on audio input.

**Key Features**:
- Rotates the sphere on the Y-axis at a configurable speed.
- Adjusts UV distortion to create a vortex effect that reacts to audio.
- Shifts color along a gradient texture to match audio amplitude changes.

**Visual Impact**:
- The sphere's dynamic color changes and motion provide a background effect that synchronizes with audio, adding depth and movement to the scene.

## Shaders

### BassReactiveParticleShader.shader
**Purpose**: Enhances particles by responding to bass frequency amplitudes, creating low-end visual effects.

### MidReactiveParticleShader.shader
**Purpose**: Reacts to mid-range frequencies, providing particle effects that highlight medium-range audio elements.

### HighReactiveParticleShader.shader
**Purpose**: Responds to high-frequency amplitudes, emphasizing sharper audio peaks with visual changes in particle properties.

### ReactiveSphereShader.shader
**Purpose**: Controls the background sphere's visuals based on audio data, including color modulation, emission, and vortex effects.

**Details**:
- Uses a gradient texture to enable smooth color transitions.
- Applies real-time audio data to modulate emission and UV distortion.

## Materials

### BassParticleMaterial
**Usage**: Paired with `BassReactiveParticleShader` to enhance bass-driven particle effects.

### MidParticleMaterial
**Usage**: Used with `MidReactiveParticleShader` for mid-frequency particle reactions.

### HighParticleMaterial
**Usage**: Applied with `HighReactiveParticleShader` to emphasize high-frequency visuals.

### ReactiveSphereMaterial
**Usage**: Attached to the sphere with `ReactiveSphereShader` for an audio-reactive background that adapts to sound data.

## How to Use This Project

1. **Clone or download the repository** from GitHub.
2. **Open the project in Unity** (compatible with Unity version 2020.3+).
3. **Add your audio source**:
   - Drag a song of choice into the MUSIC folder.
   - Click on the AudioSource in Hierarchy view.
   - Replace the AudioClip in Inspector view with your song.
4. **Run on a VR headset**:
   - This project was configured to run on Quest 2, reconfiguration might be needed for other headsets.
   - If using Quest 2, connect the device (in Developer mode) via cable to your computer
   - Go to File tab -> Build & Run
   - Run project on headset


## Song Credit
Reference song used for project construction is Dog Days Are Ove - Florence & the Machine (Fitch Remix) (BIG UP!)


## License

This project is available under the [MIT License](LICENSE), making it free to use and modify.

