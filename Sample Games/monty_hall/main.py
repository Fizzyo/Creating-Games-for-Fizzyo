#!/usr/bin/python

import pygame, sys
from pygame.locals import *
import random
import time
from time import sleep
from datetime import datetime



pygame.init()


def displayImage(x,y,currentImage):
	gameDisplay.blit(currentImage, (x,y))

def showMessage(message, x, y):
	textsurface = myfont.render(message, True, red)
	gameDisplay.blit(textsurface,(x,y))

def setImagesRandomly():
	randomList = random.sample(range(0, 3), 3)
	imageList[randomList[0]] = possibleImageList[1]
	imageList[randomList[1]] = possibleImageList[0]
	imageList[randomList[2]] = possibleImageList[1]


def displayStartImages():
	displayImage(coordinates[0][0],coordinates[0][1],doorImageList[0])
	displayImage(coordinates[1][0],coordinates[1][1],doorImageList[1])
	displayImage(coordinates[2][0],coordinates[2][1],doorImageList[2])

def displayRandomlySetImages():
	displayImage(coordinates[0][0],coordinates[0][1],imageList[0])
	displayImage(coordinates[1][0],coordinates[1][1],imageList[1])
	displayImage(coordinates[2][0],coordinates[2][1],imageList[2])

def revealImage(doorNumber):
	displayImage(coordinates[doorNumber][0], coordinates[doorNumber][1], imageList[doorNumber])

def findRatio():
	global numOfLosses, numOfWins
	if numOfLosses != 0:
		showMessage('Ratio(Wins:Losses) = '+str(1.*numOfWins/numOfLosses), x_message+500, y_message+60)

	else:
		showMessage('Ratio(Wins:Losses) = Infinite(0 Losses)', x_message+500, y_message+60)

def displayGoat(keyValue):
	randomList = random.sample(range(0, 3), 3)
	for i in range(len(imageList)):
		if randomList[i] != keyValue and imageList[randomList[i]] != openCarImage:
			revealImage(randomList[i])
			return randomList[i]

def validate(doorNumber):
	global numOfWins, numOfLosses, numOfGames
	if imageList[doorNumber] == openCarImage:
		numOfWins += 1
		showMessage('Hurray! You won a brand new Car', 1.8*x_message, y_message+540)
		showMessage('Enter q to play again.', 1.8*x_message, 620)
		# return
	else:
		numOfLosses += 1
		showMessage('Oops! You missed the brand new Car!', 1.8*x_message, y_message+540)
		showMessage('Enter q to play again.', 1.8*x_message, y_message+560)

	numOfGames += 1

def awardTheGuest(keyValue, doorNumber):
	gameOver = False
	showMessage('Would you like to swap your choice[Y/N]:', x_message, y_message+60)
	pygame.display.update()
	while not gameOver:
		for event in pygame.event.get():
			if event.type == pygame.KEYDOWN:                  
				if event.key == pygame.K_n:
					showMessage('You chose not to Swap.', x_message, y_message+80)
					pygame.display.update()
					time.sleep(sleepTime)
					validate(keyValue)
					revealImage(keyValue)
					pygame.display.update()
					gameOver = True
					break

				if event.key == pygame.K_y:
					showMessage('You chose to Swap.', x_message, y_message+80)
					pygame.display.update()
					for i in range(3):
						if i not in [doorNumber, keyValue]:
							time.sleep(sleepTime)
							validate(i)
							revealImage(i)
							pygame.display.update()
							gameOver = True
							break

			if event.type == pygame.QUIT:
				gameOver = True
				pygame.quit()
				quit()
				break

	showMessage('Number of Games = '+str(numOfGames), x_message+500, y_message)
	showMessage('Number of Wins = '+str(numOfWins), x_message+500, y_message+20)
	showMessage('Number of Losses = '+str(numOfLosses), x_message+500, y_message+40)
	findRatio()
	pygame.display.update()
				



display_width = 1400
display_height = 1300

x_message = 0.2*display_width
y_message = 60

clock = pygame.time.Clock()
random.seed(datetime.now())

pygame.font.init() 
myfont = pygame.font.SysFont(None, 25)

gameExit = False
gameOver = False
FPS = 60
sleepTime = 0
numOfWins = numOfGames = numOfLosses = 0

#-------Colors-------#
black = (0,0,0)
white = (255,255,255)
red = (255,0,0)
#-------Colors-------#

#-------Coordinates-------#
x1 = (0.1*display_width)
y1 = y2 = y3 = (0.2*display_height)
x2 = (0.4*display_width)
x3 = (0.7*display_width)
#-------Coordinates-------#

#----------------Images-----------------#
door1Image = pygame.image.load('door1.jpg')
door2Image = pygame.image.load('door2.jpg')
door3Image = pygame.image.load('door3.jpg')
openGoatImage = pygame.image.load('opengoat.jpg')
openCarImage = pygame.image.load('opencar.jpg')
#---------------------------------------#

gameDisplay = pygame.display.set_mode((display_width,display_height), RESIZABLE)
pygame.display.set_caption('Monty Hall problem')


doorImageList = [door1Image, door2Image, door3Image]
coordinates = [[x1,y1], [x2,y2], [x3,y3]]  
possibleImageList = [openCarImage, openGoatImage]
imageList = [0,1,2]





def main():
	while not gameExit:
		gameOver = False
		gameDisplay.fill(white)
		displayStartImages()
		pygame.display.update()
		setImagesRandomly()
		while not gameOver:
			showMessage("Welcome to Monty Hall Problem:", x_message, y_message)
			showMessage("Press Key 1, 2 or 3 to choose corresponding door:", x_message, y_message+20)
			pygame.display.update()

			for event in pygame.event.get():
				if event.type == pygame.KEYDOWN:
					if event.key == pygame.K_q:
						gameOver = True
						break
						
					if event.key == pygame.K_1:
						showMessage("you chose Door1!", x_message, y_message+40)
						pygame.display.update()
						time.sleep(sleepTime)
						revealedDoor = displayGoat(0)
						time.sleep(sleepTime)
						pygame.display.update()
						time.sleep(sleepTime)
						awardTheGuest(0, revealedDoor)
			  

					if event.key == pygame.K_2:
						showMessage("you chose Door2!", x_message, y_message+40)
						pygame.display.update()
						time.sleep(sleepTime)
						revealedDoor = displayGoat(1)
						time.sleep(sleepTime)
						pygame.display.update()
						time.sleep(sleepTime)
						awardTheGuest(1, revealedDoor)
				   

					if event.key == pygame.K_3:
						showMessage("you chose Door3!", x_message, y_message+40)
						pygame.display.update()
						time.sleep(sleepTime)
						revealedDoor = displayGoat(2)
						time.sleep(sleepTime)
						pygame.display.update()
						time.sleep(sleepTime)
						awardTheGuest(2, revealedDoor)
						


				if event.type == pygame.QUIT:
					pygame.quit()
					quit()
					break

		clock.tick(FPS)



main()

pygame.quit()
quit()