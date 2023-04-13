/*
 Navicat Premium Data Transfer

 Source Server         : test
 Source Server Type    : MySQL
 Source Server Version : 50741
 Source Host           : localhost:3306
 Source Schema         : zhuzhuusers

 Target Server Type    : MySQL
 Target Server Version : 50741
 File Encoding         : 65001

 Date: 07/04/2023 11:19:45
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for panologinrecord_table
-- ----------------------------
DROP TABLE IF EXISTS `panologinrecord_table`;
CREATE TABLE `panologinrecord_table`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `machine_code` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `user_type` int(11) NOT NULL,
  `ip` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `location` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `location_date` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
