# ClassroomVR 🎓 

Trabajo fin de Máster de Gabriele Petroni <br>
Máster en Ingeniería Informática - Universidad Complutense de Madrid <br>

---

**ClassroomVR** es una simulación educativa en realidad virtual que integra agentes virtuales con modelos de lenguaje (LLMs) y sistemas de voz, con el objetivo de crear entornos inmersivos de interacción alumno-profesor. Este proyecto forma parte del desarrollo del prototipo en el marco de la tesis del Máster, y se ha construido sobre la base del proyecto preexistente **DidascaliaVR**.  

---

## Objetivos del proyecto  
- Diseñar un **agente virtual inteligente** que pueda comunicarse con el usuario de manera natural mediante voz.  
- Integrar tecnologías de **Speech-to-Text (STT)** y **Text-to-Speech (TTS)** para habilitar comunicación bidireccional.  
- Implementar un flujo de interacción entre el **usuario/profesor** y un **alumno simulado** controlado por un modelo LLM.  
- Explorar la **configuración personalizada** de agentes, incluyendo nombre, contexto conversacional y modelo utilizado.  
- Sentar las bases para futuros sistemas **multiagente**, capaces de simular aulas completas con estudiantes virtuales autónomos.  

---

## Tecnologías utilizadas  
- **Unity** (motor principal de simulación).  
- **Ollama** (servidor para la ejecución local de modelos LLM).  
- **Wit.ai** (reconocimiento y síntesis de voz).  
- **C#** (scripts de gestión de agentes y lógica de interacción).  
- **Meta Voice SDK** (gestión de grabación de audio y dictado).  

---

## Funcionalidades principales  
- **Agente virtual en 3D** con animación idle básica.  
- **Interacción por voz** mediante un sistema *push-to-talk* (PTT).  
- **Gestión centralizada de agentes** con el script `AgentManager`.  
- **Configuración dinámica** mediante clases `Serializable` de Unity:  
  - Nombre del alumno.  
  - Modelo LLM utilizado.  
  - Contexto conversacional.  
  - Carpeta de logs opcional.  
  - Dirección del servidor Ollama.  
- **Registro de conversaciones** para posibles usos de *fine-tuning*.  

---

## Instalación y ejecución  
1. Clonar este repositorio:  
   ```bash
   git clone https://github.com/gabrielepetroni/ClassroomVR.git

## Autor
**Gabriele Petroni** <br>
Universidad Complutense de Madrid


