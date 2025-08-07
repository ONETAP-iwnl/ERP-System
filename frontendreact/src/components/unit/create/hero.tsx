'use client'

import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { createUnit } from '@/services/api/unitClient';
import styles from '@/styles/recipts-page/hero/hero.module.css';

export default function Hero() {
    const [inputNumber, setInputNumber] = useState('');
    const router = useRouter();

    const handleSave = async () => {
        try {
            await createUnit(inputNumber);
            router.push('/unit-page');
        } catch (error) {
            console.error('Ошибка при сохранении:', error);
            alert('Не удалось создать единицу измерения.');
        }
    };

    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Единица измерения</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={handleSave}>Сохранить</button>
            </div>
            <div className={styles.filterBox}>
                <h4 className={styles.filterText}>Наименование</h4>
                <div className={styles.inputGroup}>
                    <input
                    type='text'
                    className={styles.inputNumber}
                    value={inputNumber}
                    onChange={(e) => setInputNumber(e.target.value)}
                    />
                </div>
            </div>
        </main>
    )
}