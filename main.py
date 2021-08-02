import socket
import time
import cv2
import os
import tensorflow as tf
mp = tf.lite.Interpreter(model_path=r'C:\Users\ferna\OneDrive\Desktop\hand_landmark.tflite')

host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))

import mediapipe as mp
mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands

cap = cv2.VideoCapture(0)
with mp_hands.Hands(
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5) as hands:
  while cap.isOpened():
    success, image = cap.read()
    if not success:
      print("Ignoring empty camera frame.")
      continue

    image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)

    image.flags.writeable = False
    results = hands.process(image)

    # Draw the hand annotations on the image.
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    packet = ""
    if results.multi_hand_landmarks:
      for hand_landmarks in results.multi_hand_landmarks:
        mp_drawing.draw_landmarks(
            image, hand_landmarks, mp_hands.HAND_CONNECTIONS)
        print(hand_landmarks)
        packet = "{lHand: "+ str(hand_landmarks.landmark[0].x)+","+ str(hand_landmarks.landmark[0].y)+","+ str(hand_landmarks.landmark[0].z)
        packet += "{lThumb1: "+ str(hand_landmarks.landmark[2].x-0.05)+","+ str(hand_landmarks.landmark[2].y+0.1)+","+ str(hand_landmarks.landmark[2].z)
        packet += "{lThumb2: "+ str(hand_landmarks.landmark[3].x-0.05)+","+ str(hand_landmarks.landmark[3].y+0.1)+","+ str(hand_landmarks.landmark[3].z)
        packet += "{lThumb3: "+ str(hand_landmarks.landmark[4].x-0.05)+","+ str(hand_landmarks.landmark[4].y+0.1)+","+ str(hand_landmarks.landmark[4].z)
        packet += "{lIndex0: "+ str(hand_landmarks.landmark[5].x)+","+ str(hand_landmarks.landmark[5].y+0.18)+","+ str(hand_landmarks.landmark[5].z)
        packet += "{lIndex1: "+ str(hand_landmarks.landmark[6].x)+","+ str(hand_landmarks.landmark[6].y+0.18)+","+ str(hand_landmarks.landmark[6].z)
        packet += "{lIndex2: "+ str(hand_landmarks.landmark[7].x)+","+ str(hand_landmarks.landmark[7].y+0.18)+","+ str(hand_landmarks.landmark[7].z)
        packet += "{lIndex3: "+ str(hand_landmarks.landmark[8].x)+","+ str(hand_landmarks.landmark[8].y+0.18)+","+ str(hand_landmarks.landmark[8].z)
        packet += "{lMid0: "+ str(hand_landmarks.landmark[9].x)+","+ str(hand_landmarks.landmark[9].y+0.18)+","+ str(hand_landmarks.landmark[9].z)
        packet += "{lMid1: "+ str(hand_landmarks.landmark[10].x)+","+ str(hand_landmarks.landmark[10].y+0.18)+","+ str(hand_landmarks.landmark[10].z)
        packet += "{lMid2: "+ str(hand_landmarks.landmark[11].x)+","+ str(hand_landmarks.landmark[11].y+0.18)+","+ str(hand_landmarks.landmark[11].z)
        packet += "{lMid3: "+ str(hand_landmarks.landmark[12].x)+","+ str(hand_landmarks.landmark[12].y+0.18)+","+ str(hand_landmarks.landmark[12].z)
        packet += "{lRing0: "+ str(hand_landmarks.landmark[13].x)+","+ str(hand_landmarks.landmark[13].y+0.18)+","+ str(hand_landmarks.landmark[13].z)
        packet += "{lRing1: "+ str(hand_landmarks.landmark[14].x)+","+ str(hand_landmarks.landmark[14].y+0.18)+","+ str(hand_landmarks.landmark[14].z)
        packet += "{lRing2: "+ str(hand_landmarks.landmark[15].x)+","+ str(hand_landmarks.landmark[15].y+0.18)+","+ str(hand_landmarks.landmark[15].z)
        packet += "{lRing3: "+ str(hand_landmarks.landmark[16].x)+","+ str(hand_landmarks.landmark[16].y+0.18)+","+ str(hand_landmarks.landmark[16].z)
        packet += "{lPinky0: "+ str(hand_landmarks.landmark[17].x)+","+ str(hand_landmarks.landmark[17].y+0.18)+","+ str(hand_landmarks.landmark[17].z)
        packet += "{lPinky1: "+ str(hand_landmarks.landmark[18].x)+","+ str(hand_landmarks.landmark[18].y+0.18)+","+ str(hand_landmarks.landmark[18].z)
        packet += "{lPinky2: "+ str(hand_landmarks.landmark[19].x)+","+ str(hand_landmarks.landmark[19].y+0.18)+","+ str(hand_landmarks.landmark[19].z)
        packet += "{lPinky3: "+ str(hand_landmarks.landmark[20].x)+","+ str(hand_landmarks.landmark[20].y+0.18)+","+ str(hand_landmarks.landmark[20].z)
        print(packet)
      sock.sendall(str(packet).encode('utf-8'))
    cv2.imshow('MediaPipe Hands', image)
    if cv2.waitKey(5) & 0xFF == 27:
      break
cap.release()

