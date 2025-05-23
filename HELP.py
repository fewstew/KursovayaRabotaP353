from PIL import Image, ImageChops
import cv2

def find_contours_edges(image):
    blurred = cv2.GaussianBlur(image, (3, 3), 0)
    T, thresh_img = cv2.threshold(blurred, 150, 255, cv2.THRESH_BINARY)
    cnts, hierarchy = cv2.findContours(thresh_img, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    return cnts, hierarchy

def find_coordinates_of_cards(cnts, image):
    for i in range(0, len(cnts)):
        x, y, w, h = cv2.boundingRect(cnts[i])
        if w > 200 and h > 200:
            img_crop = image[y+30:y + h-30,
                             x+80:x + w-35]
            return img_crop
        
file = open("result.txt",'w')
file.write("1")
file.close
image_1 = cv2.imread('main.png', cv2.IMREAD_GRAYSCALE)
image_2 = cv2.imread('image.png', cv2.IMREAD_GRAYSCALE)
cnts_1, hierarchy_1 = find_contours_edges(image_1)
cnts_2, hierarchy_2 = find_contours_edges(image_2)
image_1_cropped = find_coordinates_of_cards (cnts_1, image_1)
image_2_cropped = find_coordinates_of_cards (cnts_2, image_2)
#cv2.imshow('res2',image_1_cropped)
#cv2.imshow('res3',image_2_cropped)
cv2.imwrite("image_1.jpg",image_1_cropped)
cv2.imwrite("image_2.jpg",image_2_cropped)
image_1=Image.open('image_1.jpg')
image_2=Image.open('image_2.jpg')
result=ImageChops.difference(image_1, image_2)
#result.show()
print(result.getbbox())
result.save('result.jpg')
image = cv2.imread('result.jpg', cv2.IMREAD_UNCHANGED)
blurred = cv2.GaussianBlur(image, (3, 3), 0)
T, image_get = cv2.threshold(blurred, 30, 255, cv2.THRESH_BINARY)
#cv2.imshow('sss',image_get)
cnts, hierarchy = cv2.findContours(image_get, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
if len(cnts)==0:
   file = open("result.txt",'w')
   file.write("1")
   file.close
else:
    for i in range(0, len(cnts)):
        area = cv2.arcLength(cnts[i], True)
        if area > 10:
            file = open("result.txt",'w')
            file.write("0")
            file.close
#cv2.waitKey(0)
#cv2.destroyAllWindows()