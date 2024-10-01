/**
 * Renders the page for a specific book.
 * @returns JSX.Element
 */
import { useState, useEffect } from 'react';
import { View, Text, Image, StyleSheet, Pressable, Modal, GestureResponderEvent } from 'react-native';
import { ScrollView } from 'react-native-gesture-handler';
import { Link, Stack, router, useLocalSearchParams } from 'expo-router';
import { X, BookCheck, BookX, LucideIcon, Star, StarHalf } from 'lucide-react-native';
import { BlurView } from 'expo-blur';

import TabComponent from '@/components/Tab/Tab';
import SingleBookCategory from '@/components/SingleBookCategories/SingleBookCategory';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';

import { getGenericBook, postPendingRequest, postWaitingRequest } from '@/services/apiService';

import globalStyles from '@/styles/globalStyles';

import { COLORS, FONTS, TEXTS } from '@/constants/constants';

import { truncateText } from '@/utils/textManipulation';
import { useAuth } from '@/context/AuthContext';
import { Evaluation } from '@/components/Evaluation';

export default function Page() {
  const { id } = useLocalSearchParams<{ id: string }>();
  const { authState } = useAuth();

  const globalStyle = globalStyles();

  const [book, setBook] = useState<BookType>();
  const [loading, setLoading] = useState(true);
  const [loadingRequest, setLoadingRequest] = useState(false);
  const [handlingAfterRequest, setHandlingAfterRequest] = useState(false);
  const [successful, setSuccessful] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [activeTab, setActiveTab] = useState<number>(1);

  const tabNames = [TEXTS.SYNOPSIS_TITLE, TEXTS.COPIES_TITLE, TEXTS.ABOUT_TITLE, TEXTS.REVIEWS_TITLE];

  const AFTER_REQUEST_TIME: number = 1000;
  const AFTER_FAILED_REQUEST_TIME: number = 6000;

  /**
   * Handles the press of a tab.
   * @param tabNumber
   */
  const handleTabPress = (tabNumber: number) => {
    setActiveTab(tabNumber);
  };

  /*
   * Returns the book from the API.
   * @param isbn - The ISBN of the book.
   * @returns The book from the API.
   */
  useEffect(() => {
    setLoading(true);
    getGenericBook(id)
      .then((response) => {
        setBook(response.data);
      })
      .catch((error) => console.error('Error fetching book:', error))
      .finally(() => setLoading(false));
  }, []);

  /**
   * Checks if the user library has copies of the book.
   * @returns True if the user library has copies of the book, false otherwise.
   */
  const checkIfUserLibraryHasCopies = () => {
    return book?.physicalBooks?.some((physicalBook) => physicalBook.libraryId === authState?.user?.libraryId);
  };

  /**
   * Gets the next third business day.
   * @param currentDate - The current date.
   * @returns The next third business day.
   */
  const getNextThirdBusinessDay = (currentDate: Date): string => {
    const oneDayInMilliseconds = 24 * 60 * 60 * 1000;

    // Function to check if a given day is a weekend (Saturday or Sunday)
    const isWeekend = (date: Date): boolean => {
      const day = date.getDay();
      return day === 0 /* Sunday */ || day === 6 /* Saturday */;
    };

    // Function to calculate the next business day
    const calculateNextBusinessDay = (date: Date): Date => {
      let nextDay = new Date(date.getTime() + oneDayInMilliseconds);

      while (isWeekend(nextDay)) {
        nextDay = new Date(nextDay.getTime() + oneDayInMilliseconds);
      }

      return nextDay;
    };

    // Calculate the next third business day
    let nextBusinessDay = currentDate;
    for (let i = 0; i < 3; i++) {
      nextBusinessDay = calculateNextBusinessDay(nextBusinessDay);
    }

    // Format the result as a string
    const formattedDate = nextBusinessDay.toISOString().split('T')[0];

    return formattedDate;
  };

  /**
   * Handles the after request.
   * @param successful - True if the request was successful, false otherwise.
   */
  const handleAfterRequest = (successful: boolean) => {
    setLoadingRequest(false);
    setHandlingAfterRequest(true);
    setSuccessful(successful);
  };

  /**
   * Makes a request for the book.
   */
  const makePendingRequest = () => {
    setLoadingRequest(true);
    const endDate = getNextThirdBusinessDay(new Date());

    const pendingRequest: RequestType = {
      endDate: endDate,
      userId: authState?.user?.userId!,
      isbn: book?.isbn!,
      libraryId: authState?.user?.libraryId!,
    };

    postPendingRequest(pendingRequest)
      .then(() => {
        handleAfterRequest(true);
        setTimeout(() => {
          setModalVisible(false);
          router.replace('/requested');
        }, AFTER_REQUEST_TIME);
      })
      .catch((err) => {
        handleAfterRequest(false);
        setTimeout(() => {
          setModalVisible(false);
          setHandlingAfterRequest(false);
        }, AFTER_FAILED_REQUEST_TIME);
      });
  };

  /**
   * Makes a waiting request for the book.
   */
  const makeWaitingRequest = () => {
    setLoadingRequest(true);

    const waitingRequest: RequestType = {
      userId: authState?.user?.userId!,
      isbn: book?.isbn!,
      libraryId: authState?.user?.libraryId!,
    };

    postWaitingRequest(waitingRequest)
      .then(() => {
        handleAfterRequest(true);
        setTimeout(() => {
          setModalVisible(false);
          router.replace('/requested');
        }, AFTER_REQUEST_TIME);
      })
      .catch((err) => {
        console.log(err);
        handleAfterRequest(false);
        setTimeout(() => {
          setModalVisible(false);
          setHandlingAfterRequest(false);
        }, AFTER_REQUEST_TIME);
      });
  };

  /**
   * Returns the request modal.
   * @param requestText
   * @param requestFunction
   * @returns
   */
  const requestModal = (requestText: string, requestFunction: (event: GestureResponderEvent) => void) => {
    return (
      <>
        <Pressable style={[{ alignSelf: 'flex-end', marginBottom: 5 }]} onPress={() => setModalVisible(!modalVisible)}>
          <X size={20} color="white" />
        </Pressable>

        <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular, { textAlign: 'center' }]}>
          {requestText}
        </Text>
        <Pressable
          style={[globalStyle.buttonPrimary, { marginTop: 20, marginHorizontal: 20 }]}
          onPress={requestFunction}
        >
          <Text style={[globalStyle.fontBold, styles.requestText]}>{TEXTS.CONFIRM_TEXT}</Text>
        </Pressable>
      </>
    );
  };

  /**
   * Returns the content after the request.
   * @param text
   * @param Icon
   * @param color
   * @returns
   */
  const afterRequestContent = (text: string, Icon: LucideIcon, color: string) => {
    return (
      <View style={{ alignItems: 'center', justifyContent: 'center' }}>
        <Icon size={50} color={color} />
        <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular, { textAlign: 'center' }]}>{text}</Text>
      </View>
    );
  };

  /*
   * Returns the screen after the request.
   * @returns JSX.Element
   */

  const screenAfterRequest = () => {
    if (successful) {
      return afterRequestContent(TEXTS.SUCCESSFUL_REQUEST_TEXT, BookCheck, 'green');
    } else {
      return afterRequestContent(TEXTS.UNSUCCESSFUL_REQUEST_TEXT, BookX, 'red');
    }
  };

  /**
   * Handles the number of stars for the evaluation.
   * @param averageEvaluationScore
   * @returns stars for the evaluation
   */
  const handleStarsEvaluation = (averageEvaluationScore: number) => {
    if (averageEvaluationScore === 0)
      return (
        <View style={{ flexDirection: 'row', justifyContent: 'center', marginTop: 10 }}>
          {Array(5)
            .fill(0)
            .map((_, index) => (
              <Star key={index} size={20} color="gray" fill="gray" />
            ))}
        </View>
      );

    const stars = Math.floor(averageEvaluationScore);
    const halfStar = averageEvaluationScore % 1 !== 0 ? 1 : 0;
    return (
      <View style={{ flexDirection: 'row', justifyContent: 'center', marginTop: 10 }}>
        {Array(stars)
          .fill(0)
          .map((_, index) => (
            <Star key={index} size={20} color="yellow" fill="yellow" />
          ))}
        {halfStar === 1 && <StarHalf size={20} color="yellow" fill="yellow" />}
      </View>
    );
  };

  /*
   * Returns the page for a specific book.
   * @returns JSX.Element
   */
  return (
    <ScrollView style={styles.container}>
      {loading ? (
        <>
          <Stack.Screen
            options={{
              title: '',
              headerTitleAlign: 'center',
              headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
            }}
          />
          <MyActivityIndicator />
        </>
      ) : (
        <>
          <Stack.Screen options={{ title: truncateText(book?.title!, 30) }} />
          <View style={{ alignItems: 'center' }}>
            <Image style={styles.bookImage} source={{ uri: book?.thumbnail }} />
            <Text style={[styles.titleBook, globalStyle.colorResponsive, globalStyle.fontBold]}>{book?.title}</Text>

            <Text style={[globalStyle.fontSemibold, styles.textAuthor]}>
              {book?.authors.map((author, index) => (
                <Link key={index} href={`/author/${author.authorId}`} asChild>
                  <Text key={author.authorId} style={styles.textAuthor}>
                    {author.authorName}
                    {index !== book?.authors.length - 1 && ', '}
                  </Text>
                </Link>
              ))}
            </Text>

            <View style={{ flexDirection: 'row', flexWrap: 'wrap' }}>
              {book?.categories.map((book, index) => (
                <Link key={index} href={`/category/${book.categoryId}`} asChild>
                  <Pressable>
                    <View style={styles.singleCategoryContainer} key={index}>
                      <SingleBookCategory key={book.categoryId} id={book.categoryId} name={book.categoryName} />
                    </View>
                  </Pressable>
                </Link>
              ))}
            </View>
          </View>

          {handleStarsEvaluation(book?.averageEvaluationScore!)}

          <View style={{ alignItems: 'center', justifyContent: 'center' }}>
            <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular, { fontSize: 12 }]}>
              {book?.averageEvaluationScore} from {book?.numberOfEvaluations} reviews
            </Text>
          </View>
          <TabComponent activeTab={activeTab} onTabPress={handleTabPress} tabNames={tabNames} />
          <View style={{ marginHorizontal: 10 }}>
            {activeTab === 1 && (
              <Text
                style={[globalStyle.colorResponsive, globalStyle.fontRegular, { textAlign: 'justify', marginTop: 10 }]}
              >
                {book?.description}
              </Text>
            )}
            {activeTab === 2 &&
              (book?.physicalBooks?.length === 0 ? (
                <Text style={[globalStyle.fontRegular, globalStyle.colorResponsive]}>
                  {TEXTS.REQUEST_THERE_ARE_NO_COPIES}
                </Text>
              ) : (
                <View style={{ marginTop: 10 }}>
                  <View style={styles.physicalBooksListContainer}>
                    <Text
                      style={[globalStyle.fontSemibold, styles.physicalBooksListItems, globalStyle.colorResponsive]}
                    >
                      {TEXTS.BOOK_LIBRARY_TEXT}
                    </Text>
                    <Text
                      style={[globalStyle.fontSemibold, styles.physicalBooksListItems, globalStyle.colorResponsive]}
                    >
                      {TEXTS.BOOK_QUANTITY_TEXT}
                    </Text>
                  </View>
                  {book?.physicalBooks?.map((book, index) => (
                    <View key={index} style={{ flexDirection: 'row', justifyContent: 'space-between' }}>
                      <Text style={[globalStyle.fontRegular, globalStyle.colorResponsive]}>{book.libraryAlias}</Text>
                      <Text style={[globalStyle.fontRegular, globalStyle.colorResponsive]}>{book.count}</Text>
                    </View>
                  ))}
                  <Pressable
                    style={[globalStyle.buttonPrimary, { marginTop: 20, marginHorizontal: 20 }]}
                    onPress={() => {
                      setModalVisible(true);
                    }}
                  >
                    <Text style={[globalStyle.fontBold, styles.requestText]}>{TEXTS.BOOK_REQUEST_TEXT}</Text>
                  </Pressable>
                </View>
              ))}

            {activeTab === 3 && (
              <>
                <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
                  ISBN: <Text style={globalStyle.fontRegular}>{book?.isbn}</Text>
                </Text>
                <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
                  {TEXTS.BOOK_RELEASE_DATE_TEXT} <Text style={globalStyle.fontRegular}> {book?.datePublished!}</Text>
                </Text>
                <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
                  {TEXTS.BOOK_NUMBER_OF_PAGES_TEXT} <Text style={globalStyle.fontRegular}>{book?.pageNumber}</Text>
                </Text>
                <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
                  {TEXTS.BOOK_IDIOM_TEXT} <Text style={globalStyle.fontRegular}>{book?.language.languageAlias}</Text>
                </Text>
              </>
            )}

            {activeTab === 4 && (
              <>
                <Text
                  style={[
                    styles.infoText,
                    globalStyle.colorResponsive,
                    globalStyle.fontBold,
                    { textAlign: 'center', justifyContent: 'center' },
                  ]}
                >
                  <Link href={`/evaluation/${'add;' + book?.isbn}`} asChild>
                    <Pressable style={globalStyle.buttonPrimary}>
                      <Text style={{ color: 'white' }}>Read this book? Write a review!</Text>
                    </Pressable>
                  </Link>
                </Text>
                {book?.evaluations !== null ? (
                  book?.evaluations!.map((evaluation, index) => (
                    <View key={index} style={{ marginTop: 5, marginBottom: -10 }}>
                      <Evaluation.Root>
                        <Evaluation.Content
                          evaluationDescription={evaluation.evaluationDescription}
                          evaluationScore={evaluation.evaluationScore}
                          userName={evaluation.userName}
                          isbn={book?.isbn!}
                          emittedDate={evaluation.emittedDate}
                        />
                      </Evaluation.Root>
                    </View>
                  ))
                ) : (
                  <Text
                    style={[
                      globalStyle.colorResponsive,
                      globalStyle.fontRegular,
                      { textAlign: 'center', marginTop: 20 },
                    ]}
                  >
                    There are no reviews for this book
                  </Text>
                )}
              </>
            )}
          </View>
          <Modal
            animationType="slide"
            transparent={true}
            visible={modalVisible}
            onRequestClose={() => {
              setModalVisible(!modalVisible);
            }}
          >
            <BlurView intensity={20} style={StyleSheet.absoluteFill}>
              <View style={globalStyle.centeredView}>
                <View style={globalStyle.modalView}>
                  {loadingRequest ? (
                    <MyActivityIndicator />
                  ) : handlingAfterRequest ? (
                    screenAfterRequest()
                  ) : (
                    <>
                      {checkIfUserLibraryHasCopies()
                        ? requestModal(TEXTS.PENDING_REQUEST_TEXT, makePendingRequest)
                        : requestModal(TEXTS.WAITING_REQUEST_TEXT, makeWaitingRequest)}
                    </>
                  )}
                </View>
              </View>
            </BlurView>
          </Modal>
        </>
      )}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  infoText: {
    fontSize: 14,
  },
  container: {
    flex: 1,
    flexDirection: 'column',
    marginHorizontal: 10,
    marginTop: 5,
  },
  bookImage: {
    width: 128,
    height: 192,
    borderRadius: 10,
    borderWidth: 1,
    borderColor: 'black',
  },
  physicalBooksListContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginRight: 5,
    marginBottom: 10,
  },
  singleCategoryContainer: { marginRight: 5, marginTop: 5 },
  physicalBooksListItems: {
    fontSize: 18,
    marginTop: 5,
  },
  titleBook: {
    fontSize: 18,
    textAlign: 'center',
  },
  textAuthor: {
    color: COLORS.PRIMARY_ORANGE,
    fontSize: 14,
    paddingTop: 5,
  },
  datePublished: {
    color: 'gray',
    opacity: 0.8,
    paddingTop: 5,
  },
  titlesSection: {
    fontSize: 20,
  },
  requestText: { color: 'white', fontSize: 20 },
});
