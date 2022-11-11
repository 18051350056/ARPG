/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : darkgod

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2022-07-03 12:01:20
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `acct` varchar(255) NOT NULL,
  `pass` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `power` int(11) NOT NULL,
  `coin` int(11) NOT NULL,
  `diamond` int(11) NOT NULL,
  `hp` int(11) NOT NULL,
  `ad` int(11) NOT NULL,
  `ap` int(11) NOT NULL,
  `addef` int(11) NOT NULL,
  `apdef` int(11) NOT NULL,
  `dodge` int(11) NOT NULL,
  `pierce` int(11) NOT NULL,
  `critical` int(11) NOT NULL,
  `guideid` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('35', 'cumt409', '123456', '409最帅', '22', '6666', '150', '23756297', '6352789', '20000', '2333', '1000', '200', '200', '50', '66', '90', '1001');
INSERT INTO `account` VALUES ('39', 'ahgur', '123456', '鲜于俪', '1', '0', '150', '5000', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '0');
INSERT INTO `account` VALUES ('40', 'k', '123456', '晁秀', '1', '0', '150', '5000', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '0');
INSERT INTO `account` VALUES ('41', 'sussess', '123456', '曹虹', '1', '0', '150', '5000', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '0');
INSERT INTO `account` VALUES ('42', 'gfao', '123456', '公输菲', '1', '0', '150', '5000', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '0');
INSERT INTO `account` VALUES ('43', 'gdiuag', '123456', '韶冰', '1', '0', '150', '5000', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '0');
INSERT INTO `account` VALUES ('44', 'cumt400', '123456', '公冶菲', '1', '0', '150', '5000', '500', '2000', '275', '265', '67', '43', '7', '5', '2', '1001');
