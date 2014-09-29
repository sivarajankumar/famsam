create database famsam
GO
use famsam

create table Family(
	id int not null primary key,
	name varchar(100),
	[date] datetime,
	coverURL varchar(255),
	[description] varchar(255)
)

create table UserRole(
	rolename varchar(10) primary key,
	[description] varchar(20)
)

create table [User](
	id int not null primary key,
	email varchar(100),
	[password] varchar(100),
	firstname varchar(50),
	lastname varchar(50),
	about varchar(255),
	workAt varchar(100),
	birthday datetime,
	phone varchar(20),
	infoPrivacy bit,
	[address] varchar(255),
	country varchar(20),
	avatarURL varchar(255),
	[status] varchar(10),
	[role] varchar(10) references UserRole(roleName)
)

create table GeneralPost(
	id int primary key,
	[description] varchar(255),
	lastUpdate datetime,
	postType varchar(10),
	author int references [User](id)
)

create table Story (
	id int primary key references GeneralPost(id),
	title varchar(100),
	privacy varchar(100)
)

create table [Album] (
	id int primary key references GeneralPost(id),
	title varchar(100),
)

create table [Photo] (
	id int primary key references GeneralPost(id),
	url varchar(255),
	badQuality varchar(10)
)

create table [Tag](
	name varchar(50) primary key
)

create table Neighborhood(
	familyId int references Family(id),
	neighborId int references Family(id),
	[date] datetime,
	Primary key (familyId, neighborId)
)

create table NeighborRequest(
	userId int references [User](id),
	familyId int references Family(id),
	status varchar(10),
	Primary key (userId, familyId)
)

create table [Following](
	userId int references [User](id),
	familyId int references Family(id),
	Primary key (userId, familyId)
)

create table FamilyRole(
	userId int references [User](id),
	familyId int references Family(id),
	roleName varchar(20),
	dateJoin datetime,
	Primary key (userId, familyId)
)

create table Comment(
	userId int references [User](id),
	postId int references GeneralPost(id),
	[date] datetime,
	content varchar(255),
	Primary key (userId, postId)
)

create table Sharing(
	userId int references [User](id),
	generalPostId int references GeneralPost(id),
	sharedFamilyId int references Family(id),
	[date] datetime,
	message varchar(255),
	Primary key (userId, storyId, sharedFamilyId)
)

create table Likes(
	userId int references [User](id),
	postId int references GeneralPost(id),
	Primary key (userId, postId)
)

create table Tagging(
	postId int references GeneralPost(id),
	tagName varchar(50) references Tag(name),
	Primary key (postId, tagName)
)

create table Story_Album(
	storyId int references Story(id),
	albumId int references Album(id),
	Primary key (storyId, albumId)
)

create table Album_Photo(
	albumId int references Album(id),
	photoId int references Photo(id),
	Primary key (albumId, photoId)
)

create table Report(
	userId int references [User](id),
	photoId int references Photo(id),
	[date] datetime,
	reason varchar(255),
	Primary key (userId, photoId)
)

create table [Session](
	token varchar(100) primary key,
	expired datetime,
	userId int references [User]([id])
)